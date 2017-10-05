using Newtonsoft.Json;

namespace Dapper
{
    /// <summary>
    /// Custom Type Mapper that converts string[] C# properties to JSON and vice versa.
    /// </summary>
    public class StringArrayJsonMapper : SqlMapper.TypeHandler<string[]>
    {
        public override string[] Parse(object value)
        {
            return JsonConvert.DeserializeObject<string[]>(value.ToString());
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, string[] value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }

    /// <summary>
    /// Custom Type Mapper that converts Object C# properties to JSON and vice versa.
    /// </summary>
    public class ObjectJsonMapper : SqlMapper.TypeHandler<dynamic>
    {
        public override dynamic Parse(object value)
        {
            return JsonConvert.DeserializeObject(value.ToString());
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, object value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }
}
