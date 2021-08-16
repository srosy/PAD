using Blazored.LocalStorage;
using System;
using System.Threading.Tasks;

namespace PAD.Data
{
    public static class BrowserStorage<T>
    {
        /// <summary>
        /// Gets an arbitrary object from browser storage.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <param name="storage"></param>
        /// <returns>Object<typeparamref name="T"/></returns>
        public static async Task<T> GetObject(string name, ILocalStorageService storage)
        {
            try
            {
                var obj = await storage.GetItemAsync<T>(name);
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        /// <summary>
        /// Saves an arbitraty object in browser storage.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        public static async Task SaveObject(string name, T obj, ILocalStorageService storage)
        {
            try
            {
                var existing = await storage.ContainKeyAsync(name);
                if (existing)
                {
                    await storage.RemoveItemAsync(name);
                }

                await storage.SetItemAsync(name, obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

