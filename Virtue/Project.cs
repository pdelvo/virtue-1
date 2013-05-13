using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virtue.API;

namespace Virtue
{
    public class Project : IProject
    {
        public Project()
        {
            Data = new List<SerializableKeyPair>();
        }

        public string Name { get; set; }
        public string LocalRepository { get; set; }
        public string VCSProvider { get; set; }
        public string RepositoryURL { get; set; }

        public List<SerializableKeyPair> Data { get; set; }

        public class SerializableKeyPair
        {
            public string Key;
            public object Value;
        }

        public object this[string key]
        {
            get
            {
                foreach (var item in Data)
                {
                    if (item.Key == key)
                        return item.Value;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                var existing = Data.FirstOrDefault(i => i.Key == key);
                if (existing == null)
                    Data.Remove(existing);
                Data.Add(new SerializableKeyPair
                {
                    Key = key,
                    Value = value
                });
            }
        }
    }
}
