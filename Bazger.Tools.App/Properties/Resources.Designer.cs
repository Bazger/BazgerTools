﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bazger.Tools.App.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bazger.Tools.App.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Background {
            get {
                object obj = ResourceManager.GetObject("Background", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are no videos to download, try another url.
        /// </summary>
        internal static string downloadProgressBar_All_Of_Videos_Has_Been_Downloaded_Yet {
            get {
                return ResourceManager.GetString("downloadProgressBar_All_Of_Videos_Has_Been_Downloaded_Yet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Download.
        /// </summary>
        internal static string startBtn_Download {
            get {
                return ResourceManager.GetString("startBtn_Download", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start.
        /// </summary>
        internal static string startBtn_Start {
            get {
                return ResourceManager.GetString("startBtn_Start", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stop.
        /// </summary>
        internal static string startBtn_Stop {
            get {
                return ResourceManager.GetString("startBtn_Stop", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to AIzaSyAQXaMeoVZGg5DNr5M_tgAkh28QMLb1Q6U.
        /// </summary>
        internal static string youtubeApiKey {
            get {
                return ResourceManager.GetString("youtubeApiKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter video or playlist url.
        /// </summary>
        internal static string youtubeDonwloaderUrlInfo {
            get {
                return ResourceManager.GetString("youtubeDonwloaderUrlInfo", resourceCulture);
            }
        }
    }
}
