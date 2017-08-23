using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Data;
using System.IO;

using WSWD.WmallPos.POS.FX.Shared;
using WSWD.WmallPos.FX.Shared.Exceptions;

namespace WSWD.WmallPos.FX.Shared.Utils
{
    /*=====================================================
	* 작성자     : Truong Cong Loc
	* 최초작성일 : 2006-09-21
	* 최종수정자 :
	* 최종수정일 : 
	* 주요변경로그
	======================================================*/

    #region ClassHelper

    public class ClassHelper
    {
        public static Object SafeClassLoad(string dllLocation, string className, params object[] constructorParams)
        {
            string sErrorMsg = string.Empty;
            Object genericInstance = null;

            string workingFolder = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string path = workingFolder;

            if (string.IsNullOrEmpty(dllLocation))
            {
                path = string.Empty;
            }
            else
            {
                path = Path.Combine(path, dllLocation);
            }

            Assembly asmAssemblyContainingForm = null;
            if (string.IsNullOrEmpty(path))
            {
                asmAssemblyContainingForm = Assembly.GetCallingAssembly();
            }
            else
            {
                asmAssemblyContainingForm = Assembly.LoadFrom(path);
            }

            object[] conParams = null;
            string programDll = string.Empty;
            string programClass = string.Empty;
            ParseProgramLink(className, false, out programDll, out programClass, out conParams, constructorParams);

            Type typeToLoad = asmAssemblyContainingForm.GetType(programClass);
            if (typeToLoad == null)
            {
                asmAssemblyContainingForm = Assembly.GetEntryAssembly();
                typeToLoad = asmAssemblyContainingForm.GetType(programClass);
            }

            // try to get dll name by className
            if (typeToLoad == null)
            {
                path = programClass.Substring(0, programClass.LastIndexOf('.')) + ".dll";
                string[] dlls = Directory.GetFiles(workingFolder, path, SearchOption.AllDirectories);
                if (dlls.Length > 0)
                {
                    asmAssemblyContainingForm = Assembly.LoadFrom(dlls[0]);
                    typeToLoad = asmAssemblyContainingForm.GetType(programClass);
                }
            }

            if (typeToLoad == null)
            {
                throw new ProgramNotFoundException(programClass);
            }
            else
            {
                if (conParams != null)
                {
                    genericInstance = Activator.CreateInstance(typeToLoad, conParams.ToArray());
                }
                else
                {
                    genericInstance = Activator.CreateInstance(typeToLoad);
                }

            }

            return genericInstance;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="programData"></param>
        /// <param name="includeProgData"></param>
        /// <param name="programDll"></param>
        /// <param name="programClass"></param>
        /// <param name="constructorParams"></param>
        /// <param name="addParam"></param>
        public static void ParseProgramLink(string programData, bool includeProgData,
            out string programDll, out string programClass,
            out object[] constructorParams, params object[] addParam)
        {
            programDll = string.Empty;
            programClass = string.Empty;
            constructorParams = null;

            string[] info = programData.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (info.Length < 1)
            {
                return;
            }

            List<object> ps = new List<object>();
            if (includeProgData)
            {
                ps.Add(programData);
            }

            bool hasDll = false;
            if (info[0].EndsWith("dll") || info[0].EndsWith("exe"))
            {
                hasDll = true;
            }

            object[] param = new object[1 + (addParam != null ? addParam.Length : 0) + info.Length - (hasDll ? 2 : 1)];
            param[0] = programData;

            int start = hasDll ? 2 : 1;

            for (int i = start; i < info.Length; i++)
            {
                ps.Add(info[i]);
            }

            if (addParam != null)
            {
                for (int i = 0; i < addParam.Length; i++)
                {
                    ps.Add(addParam[i]);
                }
            }

            programDll = hasDll ? info[0] : string.Empty;
            programClass = info[hasDll ? 1 : 0];
            constructorParams = ps.ToArray();
        }

        /// <summary>
        /// Method invoker
        /// </summary>
        /// <param name="dllLocation"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="classParams">class 생성자 paramss</param>
        /// <param name="methodParams">methdo parameters</param>
        public static void InvokeMethod(string dllLocation, string className, string methodName, 
            object[] classParams,
            params object[] methodParams)
        {
            object obj = null;
            if (classParams == null)
            {
                obj = SafeClassLoad(dllLocation, className);
            }
            else
            {
                obj = SafeClassLoad(dllLocation, className, classParams);
            }

            var mi = obj.GetType().GetMethod(methodName);
            mi.Invoke(obj, methodParams == null || methodParams.Length == 0 ? null : methodParams);
        }
    }


    #endregion

}
