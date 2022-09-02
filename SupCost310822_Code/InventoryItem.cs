using PX.Data.BQL;
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
       #region UsrCustomField
    [PXDBDecimal]
    [PXUIField(DisplayName="Supplier Cost")]
    public virtual decimal? UsrSupplierCost { get; set; }
    public abstract class usrSupplierCost : PX.Data.BQL.BqlDecimal.Field<usrSupplierCost> { }
    #endregion
      
       #region UsrCustomField
    [PXDBDecimal]
    [PXUIField(DisplayName="Supplier Discount %")]
    public virtual decimal? UsrSupplierDisc { get; set; }
    public abstract class usrSupplierDisc : PX.Data.BQL.BqlDecimal.Field<usrSupplierDisc> { }
        #endregion

        #region UsrCustomField
        [PXDecimal]
        [PXUIField(DisplayName = "FOB")]
        public virtual decimal? UsrFOB { get; set; }
        public abstract class usrFOB : PX.Data.BQL.BqlDecimal.Field<usrFOB> { }
        #endregion
    }
}