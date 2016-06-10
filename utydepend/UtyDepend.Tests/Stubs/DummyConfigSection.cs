using System;
using System.Collections.Generic;
using UtyDepend.Config;

namespace UtyDepend.Tests.Stubs
{
    public class DummyConfigSection : IConfigSection
    {
        public IEnumerable<IConfigSection> GetSections(string xpath)
        {
            throw new NotImplementedException();
        }

        public IConfigSection GetSection(string xpath)
        {
            throw new NotImplementedException();
        }

        public string GetString(string xpath, string defaultValue = "")
        {
            throw new NotImplementedException();
        }

        public int GetInt(string xpath, int defaultValue)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(string xpath, float defaultValue)
        {
            throw new NotImplementedException();
        }

        public bool GetBool(string xpath, bool defaultValue)
        {
            throw new NotImplementedException();
        }

        public Type GetType(string xpath)
        {
            throw new NotImplementedException();
        }

        public T GetInstance<T>(string xpath)
        {
            throw new NotImplementedException();
        }

        public T GetInstance<T>(string xpath, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}