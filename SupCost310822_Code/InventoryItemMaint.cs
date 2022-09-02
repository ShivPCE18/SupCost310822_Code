using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PX.Api;
using PX.Api.Models;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.WorkflowAPI;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.DR;
using PX.Objects.GL;
using PX.Objects.PO;
using PX.Objects.SO;
using PX.Objects.RUTROT;
using PX.Objects.Common.Discount;
using PX.SM;
using CRLocation = PX.Objects.CR.Standalone.Location;
using ItemStats = PX.Objects.IN.Overrides.INDocumentRelease.ItemStats;
using SiteStatus = PX.Objects.IN.Overrides.INDocumentRelease.SiteStatus;
using PX.Objects;
using PX.Objects.IN;
using PX.Objects.CM;
using PX.Objects.CA;

namespace PX.Objects.IN
{
    public class InventoryItemMaint_Extension : PXGraphExtension<InventoryItemMaint>
    {
        private int? location;
        public const string myCurrency = "ZAR";

        public decimal? ConvetedROE { get; private set; }
        #region Event Handlers

        protected void InventoryItem_RowSelected(PXCache cache, PXRowSelectedEventArgs e)
        {
            Vendor v = null;
            var vendorCurrency="";
            
            decimal? discountPErcent = 0;

            var row = (InventoryItem)e.Row;
            if (row != null)
            {
                //APPriceWorksheetDetail aPPrice = PXSelect<APPriceWorksheetDetail, Where<APPriceWorksheetDetail.inventoryID, Equal<Current<InventoryItem.inventoryID>>>, OrderBy<Desc<APPriceWorksheetDetail.createdDateTime>>>.Select(this.Base).TopFirst;
                POVendorInventory myVendor = this.Base.VendorItems.Search<POVendorInventory.isDefault>(true);
                if (myVendor != null)
                {
                    APPriceWorksheetDetail aPPrice = PXSelect<APPriceWorksheetDetail, Where<APPriceWorksheetDetail.inventoryID, Equal<Current<InventoryItem.inventoryID>>, And<APPriceWorksheetDetail.vendorID, Equal<Required<POVendorInventory.vendorID>>>>>.Select(this.Base, myVendor.VendorID);
                    if (aPPrice != null)
                    {
                        InventoryItemExt ext = row.GetExtension<InventoryItemExt>();
                        if (ext != null && aPPrice.PendingPrice != null)
                        {

                            APDiscountSequenceMaint sequenceMaint = PXGraph.CreateInstance<APDiscountSequenceMaint>();
                            foreach (PXResult<POVendorInventory, Vendor> item in Base.VendorItems.Select())
                            {
                                var vendor = (Vendor)item;
                                var inv = (POVendorInventory)item;
                                if (inv != null && inv.IsDefault == true)
                                {
                                    v = vendor;
                                    vendorCurrency = vendor.CuryID;
                                    location = inv.VendorLocationID;
                                }

                            }

                            if (v != null)
                            {

                                var vendorSequece = PXSelect<VendorDiscountSequence, Where<VendorDiscountSequence.vendorID, Equal<Required<VendorDiscountSequence.vendorID>>, And<VendorDiscountSequence.isActive, Equal<True>>>>.Select(this.Base, v.BAccountID);
                                if (vendorSequece != null)
                                    sequenceMaint.Sequence.Current = vendorSequece;
                            }

                            if (sequenceMaint != null)
                            {
                                var details = sequenceMaint.Details.Select().ToArray();

                                foreach (PXResult<DiscountItem, InventoryItem> item in sequenceMaint.Items.Select())
                                {
                                    var discountItem = (DiscountItem)item;
                                    if (discountItem != null)
                                    {
                                        if (discountItem.InventoryID == row.InventoryID)
                                        {
                                            for (int i = details.Length - 1; i >= 0; i--)
                                            {
                                                DiscountDetail discount = details[i];
                                                if (discount.LastDate != null && discount.LastDate.Value.Month <= DateTime.Now.Month)
                                                {
                                                    discountPErcent = discount.DiscountPercent;
                                                }
                                            }
                                        }
                                    }
                                }
                            }


                            ext.UsrSupplierCost = (decimal)aPPrice.PendingPrice;
                            ext.UsrSupDisc = discountPErcent;
                            ext.UsrItemCurrency = myCurrency;
                            ext.UsrVenCurrency = vendorCurrency;

                            cache.SetValueExt<InventoryItemExt.usrSupDisc>(row, discountPErcent);
                            //if (!string.IsNullOrEmpty( ext.UsrSupplierDisc.ToString())&& decimal.TryParse(ext.UsrSupplierDisc.ToString(), out discountPrice))
                            //{
                            if (ext.UsrSupDisc != null)
                            {
                                var cal = (decimal)ext.UsrSupplierCost / 100 * (decimal)ext.UsrSupDisc;
                                ext.UsrFOB = (decimal)aPPrice.PendingPrice - cal;

                                ext.UsrROE = GetROE(v.CuryID, ext.UsrFOB, v.CuryRateTypeID, v);

                                foreach (CSAnswers item in Base.Answers.Select())
                                {
                                    if (item.AttributeID.Trim() == "IMPORTPCT" && !string.IsNullOrEmpty(item.Value))
                                    {
                                        ext.UsrDuty = ext.UsrROE / 100 * Convert.ToDecimal(item.Value);
                                    }
                                }
                                //CSAnswers dutyPercentage = Base.Answers.Select("IMPORTPCT");



                                if (ext.UsrDuty != null)
                                    ext.UsrTotalZAR = ext.UsrROE + ext.UsrDuty;
                                ext.UsrOnCost = GetOnCost(ext.UsrTotalZAR, v);
                                ext.UsrTotalLandedCost = ext.UsrTotalZAR + ext.UsrOnCost;

                            }
                            //}
                        }
                    }
                }
            }
        }

        private decimal? GetOnCost(decimal? totalZAR, Vendor v)
        {
            decimal? res = 0;
            if (v != null)
            {
                //VendorMaint vendorMaint = PXGraph.CreateInstance<VendorMaint>();
                //VendorR vendorR = PXSelect<VendorR, Where<VendorR.acctCD, Equal<Required<VendorR.acctCD>>>>.Select(vendorMaint, v.AcctCD);
                //vendorMaint.BAccount.Current = vendorR;
                //if (vendorMaint != null)
                //{
                    foreach (CSAnswers item in Base.Answers.Select())
                    {
                        if (item.AttributeID == "VENONCOST" && !String.IsNullOrEmpty(item.Value))
                        {
                            res = totalZAR / 100 * Convert.ToDecimal(item.Value);
                        }
                    }
                //}
            }
            return res;
        }
        private decimal? GetROE(string curyID, decimal? roePrice, string rateTypeID, Vendor v)
        {
            decimal? res = 0;

            VendorMaint vendorMaint = PXGraph.CreateInstance<VendorMaint>();
            VendorR vendorR = PXSelect<VendorR, Where<VendorR.acctCD, Equal<Required<VendorR.acctCD>>>>.Select(vendorMaint, v.AcctCD);
            vendorMaint.BAccount.Current = vendorR;

            var cashAccountID = PXSelect<Location, Where<Location.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(this.Base, vendorMaint.BAccount.Current.BAccountID).TopFirst.CashAccountID;
            var accountCury = PXSelect<CashAccount, Where<CashAccount.cashAccountID, Equal<Required<CashAccount.cashAccountID>>>>.Select(this.Base, cashAccountID).TopFirst.CuryID;



            CuryRateMaint rateMaint = PXGraph.CreateInstance<CuryRateMaint>();
            CuryRateFilter filter = new CuryRateFilter
            {
                ToCurrency = accountCury,
                EffDate = DateTime.Now
            };
            rateMaint.Filter.Current = filter;
            //CurrencyRate2 item = rateMaint.CuryRateRecordsEffDate.Select("USD");




            foreach (CurrencyRate2 item in rateMaint.CuryRateRecordsEffDate.Select())
            {

                if (item.CuryRateType.Trim().ToLower() == "other" && item.FromCuryID == v.CuryID)
                    res = roePrice / item.CuryRate;
            }
            // }
            // }
            return res;
        }

        protected void InventoryItem_UsrSupDisc_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {

            var row = (InventoryItem)e.Row;
            if (row != null)
            {
                APPriceWorksheetDetail aPPrice = PXSelect<APPriceWorksheetDetail, Where<APPriceWorksheetDetail.inventoryID, Equal<Current<InventoryItem.inventoryID>>>, OrderBy<Desc<APPriceWorksheetDetail.createdDateTime>>>.Select(this.Base).TopFirst;
                if (aPPrice != null)
                {
                    InventoryItemExt ext = row.GetExtension<InventoryItemExt>();
                    if (ext != null && aPPrice.PendingPrice != null)
                    {
                        //if (!string.IsNullOrEmpty( ext.UsrSupplierDisc.ToString())&& decimal.TryParse(ext.UsrSupplierDisc.ToString(), out discountPrice))
                        //{
                        if (ext.UsrSupDisc != null&& ext.UsrSupplierCost!=null)
                        {
                            var cal = (decimal)ext.UsrSupplierCost / 100 * (decimal)ext.UsrSupDisc;
                            ext.UsrFOB = (decimal)aPPrice.PendingPrice - cal;
                        }
                        //}
                    }
                }
            }
        }



        #endregion
    }
}