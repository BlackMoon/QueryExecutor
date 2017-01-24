using System.Linq;
using System.Web.OData;
using System.Web.OData.Formatter.Serialization;
using Microsoft.OData.Core;

namespace queryExecutor.OData
{
    /// <summary>
    /// OData серилизатор (Игнорирование null значений)
    /// </summary>
    public class SkipNullValueEntitySerializer : ODataEntityTypeSerializer
    {
        public SkipNullValueEntitySerializer(ODataSerializerProvider serializerProvider) : base(serializerProvider)
        {
        }

        public override ODataEntry CreateEntry(SelectExpandNode selectExpandNode, EntityInstanceContext entityInstanceContext)
        {
            ODataEntry entry = base.CreateEntry(selectExpandNode, entityInstanceContext);

            // Remove any properties which are null
            entry.Properties = entry.Properties.Where(property => property.Value != null);
            return entry;
        }
    }
}