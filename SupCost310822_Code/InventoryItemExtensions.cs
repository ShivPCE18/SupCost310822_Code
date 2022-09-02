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
    public class InventoryItemExt2 : PXCacheExtension<PX.Objects.IN.InventoryItem>
    {
        #region UsrFOB
        [PXDBDecimal]
        [PXUIField(DisplayName = "FOB")]

        public virtual Decimal? UsrFOB { get; set; }
        public abstract class usrFOB : PX.Data.BQL.BqlDecimal.Field<usrFOB> { }
        #endregion

        #region UsrROE
        [PXDBDecimal]
        [PXUIField(DisplayName = "ROE")]

        public virtual Decimal? UsrROE { get; set; }
        public abstract class usrROE : PX.Data.BQL.BqlDecimal.Field<usrROE> { }
        #endregion

        #region UsrDuty
        [PXDBDecimal]
        [PXUIField(DisplayName = "Duty")]

        public virtual Decimal? UsrDuty { get; set; }
        public abstract class usrDuty : PX.Data.BQL.BqlDecimal.Field<usrDuty> { }
        #endregion

        #region UsrTotalZAR
        [PXDBDecimal]
        [PXUIField(DisplayName = "Total ZAR")]

        public virtual Decimal? UsrTotalZAR { get; set; }
        public abstract class usrTotalZAR : PX.Data.BQL.BqlDecimal.Field<usrTotalZAR> { }
        #endregion

        #region UsrOnCost
        [PXDBDecimal]
        [PXUIField(DisplayName = "On Cost")]

        public virtual Decimal? UsrOnCost { get; set; }
        public abstract class usrOnCost : PX.Data.BQL.BqlDecimal.Field<usrOnCost> { }
        #endregion

        #region UsrSupDisc
        [PXDBDecimal]
        [PXUIField(DisplayName = "Supplier Disc")]

        public virtual Decimal? UsrSupDisc { get; set; }
        public abstract class usrSupDisc : PX.Data.BQL.BqlDecimal.Field<usrSupDisc> { }
        #endregion

        #region UsrTotalLandedCost
        [PXDBDecimal]
        [PXUIField(DisplayName = "Total Landed Cost")]

        public virtual Decimal? UsrTotalLandedCost { get; set; }
        public abstract class usrTotalLandedCost : PX.Data.BQL.BqlDecimal.Field<usrTotalLandedCost> { }
        #endregion

        //#region UsrSupplierDisc
        //[PXDBDecimal]
        //[PXUIField(DisplayName = "Supplier Disc %")]

        //public virtual Decimal? UsrSupplierDisc { get; set; }
        //public abstract class usrSupplierDisc : PX.Data.BQL.BqlDecimal.Field<usrSupplierDisc> { }
        //#endregion

        #region UsrSupplierCost
        [PXDBDecimal]
        [PXUIField(DisplayName = "Supplier Cost")]

        public virtual Decimal? UsrSupplierCost { get; set; }
        public abstract class usrSupplierCost : PX.Data.BQL.BqlDecimal.Field<usrSupplierCost> { }
        #endregion
    }
}