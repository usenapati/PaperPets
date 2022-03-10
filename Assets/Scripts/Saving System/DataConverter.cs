/*using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;

// This is being done because newtonsoft stores keys as just string rather than as a json object
// -- we need to save the information inside the keys - saving them as just string will lose info.
// Need to decorate WorldSim with this [TypeConverter(typeof(WorldSimConverter))]
public class WorldSimConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        string jsonString = value as string;

        if (jsonString == null)
            return base.ConvertFrom(context, culture, value);

        return JsonConvert.DeserializeObject(jsonString);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        var casted = value as WorldSim;
        return destinationType == typeof(string) && casted != null
            ? JsonConvert.SerializeObject(casted)
            : base.ConvertTo(context, culture, value, destinationType);
    }
}

*/