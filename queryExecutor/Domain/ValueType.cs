// ReSharper disable InconsistentNaming

namespace queryExecutor.Domain
{
    public enum EValueType : long
    {
        NUMBER = 915001,
        DATE = 915002,
        VARCHAR = 915003,
        OBJECT = 915004,
        CHAR = 915005,
        BOOLEAN = 915007,
        LOGICAL = 915008,
        TEMPTEXT = 915009,
        YEAR = 915010,
        BLOB = 915011,
        TIMESTAMP = 915012,
        CLOB = 915013,
        //
        UNKNOWN = -1
    }
}