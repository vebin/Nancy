﻿namespace Nancy.Conventions
{
    using System;
    using System.Collections.Generic;
    using ViewEngines;

    /// <summary>
    /// Defines the default static contents conventions.
    /// </summary>
    public class DefaultViewLocationConventions : IConvention
    {
        /// <summary>
        /// Initialise any conventions this class "owns".
        /// </summary>
        /// <param name="conventions">Convention object instance.</param>
        public void Initialise(NancyConventions conventions)
        {
            ConfigureViewLocationConventions(conventions);
        }

        /// <summary>
        /// Asserts that the conventions that this class "owns" are valid.
        /// </summary>
        /// <param name="conventions">Conventions object instance.</param>
        /// <returns>Tuple containing true/false for valid/invalid, and any error messages.</returns>
        public Tuple<bool, string> Validate(NancyConventions conventions)
        {
            if (conventions.ViewLocationConventions == null)
            {
                return Tuple.Create(false, "The view conventions cannot be null.");
            }

            return (conventions.ViewLocationConventions.Count > 0) ? 
                Tuple.Create(true, string.Empty) :
                Tuple.Create(false, "The view conventions cannot be empty.");
        }

        private static void ConfigureViewLocationConventions(NancyConventions conventions)
        {
            conventions.ViewLocationConventions = new List<Func<string, object, ViewLocationContext, string>>
            {
                // 0 Handles: views / *modulepath* / *modulename* / *viewname*
                (viewName, model, viewLocationContext) =>{
                    if (string.IsNullOrEmpty(viewLocationContext.ModulePath))
                    {
                        return string.Empty;
                    }

                    var path = viewLocationContext.ModulePath.TrimStart(new[] { '/' });

                    return  string.Concat("views/", path, "/", viewLocationContext.ModuleName, "/", viewName);
                },

                // 1 Handles: *modulepath* / *modulename* / *viewname*
                (viewName, model, viewLocationContext) =>{
                    if (string.IsNullOrEmpty(viewLocationContext.ModulePath))
                    {
                        return string.Empty;
                    }

                    var path = viewLocationContext.ModulePath.TrimStart(new[] { '/' });

                    return  string.Concat(path, "/", viewLocationContext.ModuleName, "/", viewName);
                },

                // 2 Handles: views / *modulepath* / *viewname*
                (viewName, model, viewLocationContext) =>{
                    return string.IsNullOrEmpty(viewLocationContext.ModulePath) ? string.Empty : string.Concat("views/", viewLocationContext.ModulePath.TrimStart(new[] {'/'}), "/", viewName);
                },

                // 3 Handles: *modulepath* / *viewname*
                (viewName, model, viewLocationContext) =>{
                    return string.IsNullOrEmpty(viewLocationContext.ModulePath) ? string.Empty : string.Concat(viewLocationContext.ModulePath.TrimStart(new[] { '/' }), "/", viewName);
                },

                // 4 Handles: views / *modulename* / *viewname*
                (viewName, model, viewLocationContext) => {
                    return string.Concat("views/", viewLocationContext.ModuleName, "/", viewName);
                },

                // 5 Handles: *modulename* / *viewname*
                (viewName, model, viewLocationContext) => {
                    return string.Concat(viewLocationContext.ModuleName, "/", viewName);
                },

                // 6 Handles: views / *viewname*
                (viewName, model, viewLocationContext) => {
                    return string.Concat("views/", viewName);
                },

                // 7 Handles: *viewname*
                (viewName, model, viewLocationContext) => {
                    return viewName;
                }
            };
        }
    }
}