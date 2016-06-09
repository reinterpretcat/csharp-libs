using System;
using System.Collections.Generic;

namespace DependencyNet.Config
{
    /// <summary> Represens a config entry. </summary>
    public interface IConfigSection
    {
        /// <summary> Returns the set of ConfigSections. </summary>
        /// <param name="xpath">xpath</param>
        IEnumerable<IConfigSection> GetSections(string xpath);

        /// <summary> Returns JsonConfigSection. </summary>
        /// <param name="xpath">xpath</param>
        /// <returns>IConfigSection.</returns>
        IConfigSection GetSection(string xpath);

        /// <summary> Returns string.</summary>
        /// <param name="xpath">xpath.</param>
        /// <param name="defaultValue"></param>
        /// <returns>String value.</returns>
        string GetString(string xpath, string defaultValue = "");

        /// <summary> Returns int. </summary>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns>Int value.</returns>
        int GetInt(string xpath, int defaultValue);

        /// <summary> Returns float. </summary>
        /// <param name="xpath">xpath</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Float value.</returns>
        float GetFloat(string xpath, float defaultValue);

        /// <summary> Returns bool. </summary>
        /// <param name="xpath">xpath</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Boolean.</returns>
        bool GetBool(string xpath, bool defaultValue);

        /// <summary> Returns type object. </summary>
        /// <param name="xpath">xpath.</param>
        /// <returns>Type.</returns>
        Type GetType(string xpath);

        /// <summary> Returns the instance of T. </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="xpath">xpath.</param>
        /// <returns>Instance.</returns>
        T GetInstance<T>(string xpath);

        /// <summary> Returns the instance of T. </summary>
        /// <typeparam name="T">Instance type.</typeparam>
        /// <param name="xpath">xpath</param>
        /// <param name="args">Constructor parameters.</param>
        /// <returns>Instance.</returns>
        T GetInstance<T>(string xpath, params object[] args);
    }
}