using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Desktop.AddIns;

namespace t03ArcExtend
{
    /// <summary>
    /// Summary description for ArcGISToolbar1.
    /// </summary>
    [Guid("2a5f2ff7-a742-4443-b8e6-b45fde9253ba")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("t03ArcExtend.ArcGISToolbar1")]
    public sealed class ArcGISToolbar1 : BaseToolbar
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommandBars.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommandBars.Unregister(regKey);
        }

        #endregion
        #endregion

        public ArcGISToolbar1()
        {
            //
            // TODO: Define your toolbar here by adding items
            //
            AddItem("esriArcMapUI.ZoomInTool");
            BeginGroup(); //Separator
            AddItem("{FBF8C3FB-0480-11D2-8D21-080009EE4E51}", 1); //undo command
            AddItem(new Guid("FBF8C3FB-0480-11D2-8D21-080009EE4E51"), 2); //redo command
            AddItem("t03ArcExtend.Command1");
        }

        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "大数据处理";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "ArcGISToolbar1";
            }
        }
    }
    /*
    internal static class ArcMap
    {
        private static IApplication s_app = null;
        private static IDocumentEvents_Event s_docEvent;

        public static IApplication Application
        {
            get
            {
                if (s_app == null)
                    s_app = Internal.AddInStartupObject.GetHook<IMxApplication>() as IApplication;

                return s_app;
            }
        }

        public static IMxDocument Document
        {
            get
            {
                if (Application != null)
                    return Application.Document as IMxDocument;

                return null;
            }
        }
        public static IMxApplication ThisApplication
        {
            get { return Application as IMxApplication; }
        }
        public static IDockableWindowManager DockableWindowManager
        {
            get { return Application as IDockableWindowManager; }
        }
        public static IDocumentEvents_Event Events
        {
            get
            {
                s_docEvent = Document as IDocumentEvents_Event;
                return s_docEvent;
            }
        }
    }

    namespace Internal
    {
        [StartupObjectAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public sealed partial class AddInStartupObject : AddInEntryPoint
        {
            private static AddInStartupObject _sAddInHostManager;
            private List<object> m_addinHooks = null;

            [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
            public AddInStartupObject()
            {
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
            protected override bool Initialize(object hook)
            {
                bool createSingleton = _sAddInHostManager == null;
                if (createSingleton)
                {
                    _sAddInHostManager = this;
                    m_addinHooks = new List<object>();
                    m_addinHooks.Add(hook);
                }
                else if (!_sAddInHostManager.m_addinHooks.Contains(hook))
                    _sAddInHostManager.m_addinHooks.Add(hook);

                return createSingleton;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
            protected override void Shutdown()
            {
                _sAddInHostManager = null;
                m_addinHooks = null;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
            internal static T GetHook<T>() where T : class
            {
                if (_sAddInHostManager != null)
                {
                    foreach (object o in _sAddInHostManager.m_addinHooks)
                    {
                        if (o is T)
                            return o as T;
                    }
                }

                return null;
            }

            // Expose this instance of Add-in class externally
            public static AddInStartupObject GetThis()
            {
                return _sAddInHostManager;
            }
        }
    }
    */
}