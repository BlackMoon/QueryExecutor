using System.Web.OData.Formatter.Serialization;
using Microsoft.OData.Edm;

namespace queryExecutor.OData
{
    /// <summary>
    /// Провайдер Odata серилизации. (Игнорирование null значений)
    /// </summary>
    public class SkipNullValueODataSerializerProvider : DefaultODataSerializerProvider
    {
        private readonly SkipNullValueEntitySerializer _skipNullValueEntitySerializer;

        public SkipNullValueODataSerializerProvider()
        {
            _skipNullValueEntitySerializer = new SkipNullValueEntitySerializer(this);
        }

        public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
        {
            return edmType.IsEntity() ? _skipNullValueEntitySerializer : base.GetEdmTypeSerializer(edmType);
        }
    }
}