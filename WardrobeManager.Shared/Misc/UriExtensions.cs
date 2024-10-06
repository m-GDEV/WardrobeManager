using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WardrobeManager.Shared.Misc;


// used to convert DTO objects from objects to a query string 
// primarily when you need to send data in a GET request
public static class UriExtensions
{
    public static Uri AddToQuery<T>(this Uri requestUri, T dto)
    {
        Type t = typeof(T);
        var properties = t.GetProperties();
        var dictionary = properties.ToDictionary(
          info => info.Name, info => info.GetValue(dto, null).ToString());
        var formContent = new FormUrlEncodedContent(dictionary);

        var uriBuilder = new UriBuilder(
          requestUri)
        { Query = formContent.ReadAsStringAsync().Result };

        return uriBuilder.Uri;
    }
}
