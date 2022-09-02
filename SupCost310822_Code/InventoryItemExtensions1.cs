using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.Common.Extensions;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.DR;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.IN.Matrix.Attributes;
using PX.Objects.IN.Matrix.Graphs;
using PX.Objects.IN;
using PX.Objects.TX;
using PX.Objects;
using PX.TM;
using System.Collections.Generic;
using System;

namespace PX.Objects.IN
{
  public class InventoryItemExt : PXCacheExtension<PX.Objects.IN.InventoryItem>
  {
    #region UsrDuty
    [PXDBDecimal]
    [PXUIField(DisplayName="Duty")]

    public virtual Decimal? UsrDuty { get; set; }
    public abstract class usrDuty : PX.Data.BQL.BqlDecimal.Field<usrDuty> { }
    #endregion

    #region UsrFOB
    [PXDBDecimal]
    [PXUIField(DisplayName="FOB")]

    public virtual Decimal? UsrFOB { get; set; }
    public abstract class usrFOB : PX.Data.BQL.BqlDecimal.Field<usrFOB> { }
    #endregion

    #region UsrSupplierCost
    [PXDBDecimal]
    [PXUIField(DisplayName="Supplier Cost")]

    public virtual Decimal? UsrSupplierCost { get; set; }
    public abstract class usrSupplierCost : PX.Data.BQL.BqlDecimal.Field<usrSupplierCost> { }
        #endregion

        #region UsrVenCurrency
        [PXDBString]
        [PXUIField(DisplayName = "")]

        public virtual string UsrVenCurrency { get; set; }
        public abstract class usrVenCurrency : PX.Data.BQL.BqlString.Field<usrVenCurrency> { }
        #endregion

        #region UsrItemCurrency
        [PXDBString]
        [PXUIField(DisplayName = "")]

        public virtual string UsrItemCurrency { get; set; }
        public abstract class usrItemCurrency : PX.Data.BQL.BqlString.Field<usrItemCurrency> { }
        #endregion

        #region UsrSupDisc
        [PXDBDecimal]
    [PXUIField(DisplayName="Supplier Disc")]

    public virtual Decimal? UsrSupDisc { get; set; }
    public abstract class usrSupDisc : PX.Data.BQL.BqlDecimal.Field<usrSupDisc> { }
    #endregion

    #region UsrOnCost
    [PXDBDecimal]
    [PXUIField(DisplayName="On Cost")]

    public virtual Decimal? UsrOnCost { get; set; }
    public abstract class usrOnCost : PX.Data.BQL.BqlDecimal.Field<usrOnCost> { }
    #endregion

    #region UsrTotalZAR
    [PXDBDecimal]
    [PXUIField(DisplayName="Total ZAR")]

    public virtual Decimal? UsrTotalZAR { get; set; }
    public abstract class usrTotalZAR : PX.Data.BQL.BqlDecimal.Field<usrTotalZAR> { }
    #endregion

    #region UsrROE
    [PXDBDecimal]
    [PXUIField(DisplayName="ROE")]

    public virtual Decimal? UsrROE { get; set; }
    public abstract class usrROE : PX.Data.BQL.BqlDecimal.Field<usrROE> { }
    #endregion

    #region UsrTotalLandedCost
    [PXDBDecimal]
    [PXUIField(DisplayName="Total Landed cost")]

    public virtual Decimal? UsrTotalLandedCost { get; set; }
    public abstract class usrTotalLandedCost : PX.Data.BQL.BqlDecimal.Field<usrTotalLandedCost> { }
    #endregion
  }
}