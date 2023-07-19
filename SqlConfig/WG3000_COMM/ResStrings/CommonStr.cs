namespace WG3000_COMM.ResStrings
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class CommonStr
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal CommonStr()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("WG3000_COMM.ResStrings.CommonStr", typeof(CommonStr).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static string strAccessHigh
        {
            get
            {
                return ResourceManager.GetString("strAccessHigh", resourceCulture);
            }
        }

        internal static string strConnectFailed
        {
            get
            {
                return ResourceManager.GetString("strConnectFailed", resourceCulture);
            }
        }

        internal static string strConnectSuccessfully
        {
            get
            {
                return ResourceManager.GetString("strConnectSuccessfully", resourceCulture);
            }
        }

        internal static string strCreateNewDatabaseCheck
        {
            get
            {
                return ResourceManager.GetString("strCreateNewDatabaseCheck", resourceCulture);
            }
        }

        internal static string strDBExist
        {
            get
            {
                return ResourceManager.GetString("strDBExist", resourceCulture);
            }
        }

        internal static string strDBNotExist
        {
            get
            {
                return ResourceManager.GetString("strDBNotExist", resourceCulture);
            }
        }

        internal static string strFailed
        {
            get
            {
                return ResourceManager.GetString("strFailed", resourceCulture);
            }
        }

        internal static string strFailedToBackupDatabase
        {
            get
            {
                return ResourceManager.GetString("strFailedToBackupDatabase", resourceCulture);
            }
        }

        internal static string strDatabaseCreationSuccess
        {
            get
            {
                return ResourceManager.GetString("strDatabaseCreationSuccess", resourceCulture);
            }
        }

        internal static string strFailedToCreateDatabase
        {
            get
            {
                return ResourceManager.GetString("strFailedToCreateDatabase", resourceCulture);
            }
        }

        internal static string strFailedToRestoreDatabase
        {
            get
            {
                return ResourceManager.GetString("strFailedToRestoreDatabase", resourceCulture);
            }
        }

        internal static string strFileNotExist
        {
            get
            {
                return ResourceManager.GetString("strFileNotExist", resourceCulture);
            }
        }

        internal static string strMsgAbort
        {
            get
            {
                return ResourceManager.GetString("strMsgAbort", resourceCulture);
            }
        }

        internal static string strMsgCancel
        {
            get
            {
                return ResourceManager.GetString("strMsgCancel", resourceCulture);
            }
        }

        internal static string strMsgIgnore
        {
            get
            {
                return ResourceManager.GetString("strMsgIgnore", resourceCulture);
            }
        }

        internal static string strMsgNo
        {
            get
            {
                return ResourceManager.GetString("strMsgNo", resourceCulture);
            }
        }

        internal static string strMsgOK
        {
            get
            {
                return ResourceManager.GetString("strMsgOK", resourceCulture);
            }
        }

        internal static string strMsgRetry
        {
            get
            {
                return ResourceManager.GetString("strMsgRetry", resourceCulture);
            }
        }

        internal static string strMsgYes
        {
            get
            {
                return ResourceManager.GetString("strMsgYes", resourceCulture);
            }
        }

        internal static string strRestoreDatabaseCheck
        {
            get
            {
                return ResourceManager.GetString("strRestoreDatabaseCheck", resourceCulture);
            }
        }

        internal static string strRestoreDatabaseCheckFileName
        {
            get
            {
                return ResourceManager.GetString("strRestoreDatabaseCheckFileName", resourceCulture);
            }
        }

        internal static string strSelectMsAccessDatabaseCheck
        {
            get
            {
                return ResourceManager.GetString("strSelectMsAccessDatabaseCheck", resourceCulture);
            }
        }

        internal static string strUpgradeDatabaseFromMsAccessCheck
        {
            get
            {
                return ResourceManager.GetString("strUpgradeDatabaseFromMsAccessCheck", resourceCulture);
            }
        }

        internal static string strVersion
        {
            get
            {
                return ResourceManager.GetString("strVersion", resourceCulture);
            }
        }

        internal static string strBackupDatabaseSuccess
        {
            get
            {
                return ResourceManager.GetString("strBackupDatabaseSuccess", resourceCulture);
            }
        }

        internal static string strSaveOK
        {
            get
            {
                return ResourceManager.GetString("strSaveOK", resourceCulture);
            }
        }

        internal static string strUpgradeSuccess
        {
            get
            {
                return ResourceManager.GetString("strUpgradeSuccess", resourceCulture);
            }
        }

        internal static string strUseAccessSuccess
        {
            get
            {
                return ResourceManager.GetString("strUseAccessSuccess", resourceCulture);
            }
        }

        internal static string strRestoreSuccess
        {
            get
            {
                return ResourceManager.GetString("strRestoreSuccess", resourceCulture);
            }
        }

        internal static string strApplicationAlreadyRunning
        {
            get
            {
                return ResourceManager.GetString("strApplicationAlreadyRunning", resourceCulture);
            }
        }
    }
}

