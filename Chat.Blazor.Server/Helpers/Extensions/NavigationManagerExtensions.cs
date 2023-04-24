using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace Chat.Blazor.Server.Helpers.Extensions
{
    public static class NavigationManagerExtensions
    {

        public static T ExtractQueryValueOrSetDefault<T>(this NavigationManager navManager, string key, object? defaultValue = null)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri);
            QueryHelpers.ParseQuery(uri.Query)
                .TryGetValue(key, out var queryValue);

            //if (string.IsNullOrEmpty(queryValue))
            //{
            //    return default(T);
            //}

            if (typeof(T).Equals(typeof(int)))
            {
                var isSuccess = int.TryParse(queryValue, out int result);
                if (isSuccess)
                {
                    return (T)(object)result;
                }

                if (defaultValue != null)
                {
                    return (T)defaultValue;
                }

            }

            if (typeof(T).Equals(typeof(string)))
            {
                if (!string.IsNullOrEmpty(queryValue))
                {
                    return (T)(object)queryValue.ToString();
                }

                if (defaultValue != null)
                {
                    return (T)(object)defaultValue.ToString();
                }

            }
            return default;

        }


    }

    
}
