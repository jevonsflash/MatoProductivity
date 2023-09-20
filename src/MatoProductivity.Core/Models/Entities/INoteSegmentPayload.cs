namespace MatoProductivity.Core.Models.Entities
{
    public interface INoteSegmentPayload
    {
        string Key { get; set; }
        string StringValue { get; }
        byte[] Value { get; set; }
        string ValueType { get; set; }

        T GetConcreteValue<T>() where T : struct;
        string GetStringValue();
        void SetStringValue(string value);
    }
}