using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace queryExecutor.DbManager.Oracle.Udt.TVariantNamedList
{
    /// <summary>
    /// Summary description for TVariantNamedList
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TVariantNamedList : INullable, IOracleCustomType
    {
        private bool _isNull;

        [OracleObjectMapping("ITEMS")] 
        public queryExecutor.DbManager.Oracle.Udt.TVariantNamed.TVariantNamed[] Items { get; private set; }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, "ITEMS", Items);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            Items = (queryExecutor.DbManager.Oracle.Udt.TVariantNamed.TVariantNamed[])OracleUdt.GetValue(con, pUdt, "ITEMS");
        }

        /// <summary>
        /// Устанавливает элемент по имени 
        /// </summary>
        public void SetItem(ref TVariantNamedList self, queryExecutor.DbManager.Oracle.Udt.TVariantNamed.TVariantNamed item, OracleConnection con)
        {
            using (OracleCommand cmd = new OracleCommand("T_VARIANT_NAMED_LIST.SET_ITEM", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;

                OracleParameter pSelf = new OracleParameter("self", OracleDbType.Object, ParameterDirection.InputOutput)
                {
                    UdtTypeName = "PUBLIC.T_VARIANT_NAMED_LIST",
                    Value = self
                };

                OracleParameter pItem = new OracleParameter("pItem", OracleDbType.Object, ParameterDirection.Input)
                {
                    UdtTypeName = "PUBLIC.T_VARIANT_NAMED",
                    Value = item
                };

                cmd.Parameters.AddRange(new[] { pSelf, pItem });
                cmd.ExecuteNonQuery();

                self = pSelf.Value as TVariantNamedList;
            }    
        }

        /// <summary>
        /// Устанавливает список элементов
        /// </summary>
        public void SetItems(ref TVariantNamedList self, queryExecutor.DbManager.Oracle.Udt.TVariantNamed.TVariantNamed [] items, OracleConnection con)
        {
            using (OracleCommand cmd = new OracleCommand("T_VARIANT_NAMED_LIST.SET_ITEMS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;

                OracleParameter pSelf = new OracleParameter("self", OracleDbType.Object, ParameterDirection.InputOutput)
                {
                    UdtTypeName = "PUBLIC.T_VARIANT_NAMED_LIST",
                    Value = self
                };

                OracleParameter pItems = new OracleParameter("pItems", OracleDbType.Array, ParameterDirection.Input)
                {
                    UdtTypeName = "PUBLIC.T_VARIANT_NAMED_TABLE",
                    Size = items.Length,
                    Value = items
                };

                cmd.Parameters.AddRange(new [] { pSelf, pItems });
                cmd.ExecuteNonQuery();

                self = pSelf.Value as TVariantNamedList;
            }        
        }

        public bool IsNull 
        {
            get { return _isNull; }
        }

        // TVariantNamed.Null is used to return a NULL TVariantNamed object
        public static TVariantNamedList Null
        {
            get { return new TVariantNamedList { _isNull = true }; }
        }

        public static TVariantNamedList Create(OracleConnection con, queryExecutor.DbManager.Oracle.Udt.TVariantNamed.TVariantNamed[] items)
        {
            var obj = new TVariantNamedList();
            obj.SetItems(ref obj, items, con);

            return obj;
        }

        public static TVariantNamedList Create()
        {
            return new TVariantNamedList();
        }

        private TVariantNamedList()
        {
        
        }

    }
}