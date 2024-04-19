using System.Text.Json;

namespace AdamBlocklyLibrary
{
    public class Scripts
    {
        #region Const

        public const string BlocksCompressedSrc = "source/blocks_compressed.js";
        public const string JavascriptCompressedSrc = "source/javascript_compressed.js";
        public const string PythonCompressedSrc = "source/python_compressed.js";
        
        public const string DateTimeBlockSrc = "source/blocks/date_time.js";
        public const string DateTimePytnonGenSrc = "source/generators/python/date_time.js";

        public const string ThreadBlockSrc = "source/blocks/thread.js";
        public const string ThreadPytnonGenSrc = "source/generators/python/thread.js";

        public const string SystemsBlockSrc = "source/blocks/python_system_call.js";
        public const string SystemsPythonGenSrc = "source/generators/python/python_system_call.js";

        public const string AdamTwoBlockSrc = "source/blocks/adam_two.js";
        public const string AdamTwoPytnonGenSrc = "source/generators/python/adam_two.js";

        public const string AdamThreeBlockSrc = "source/blocks/adam_three.js";
        public const string AdamThreePytnonGenSrc = "source/generators/python/adam_three.js";

        public const string AdamCommonBlockSrc = "source/blocks/adam_common.js";
        public const string AdamCommonPytnonGenSrc = "source/generators/python/adam_common.js";

        public const string BlockLanguageRu = "source/msg/js/ru.js";
        public const string BlockLanguageEn = "source/msg/js/en.js";
        public const string BlockLanguageZnHans = "source/msg/js/zh-hans.js";

        public const string ListenerCreatePythonCode = "workspace.addChangeListener(event => {Blockly.JavaScript.STATEMENT_PREFIX = null;" +
            "const code = Blockly.Python.workspaceToCode(workspace);sendMessage('sendSourceCode', code);});";

        public const string ListenerSavedBlocks = "function onchange(event){ saveWorkspace(Blockly.mainWorkspace); } workspace.addChangeListener(onchange);";
        public const string RestoreSavedBlocks = "loadSavedWorkspace(getSavedWorkspace())";
        

        public const string ShadowEnable = "function shadowEnable(){ " +
            "if(document.getElementById('shadow') == null) {" +
            "var darkLayer = document.createElement('div'); " +
            "darkLayer.id = 'shadow'; " +
            "document.body.appendChild(darkLayer); }} shadowEnable();";
        
        public const string ShadowDisable = "function shadowDisable(){ " +
            "var darkLayer = document.getElementById('shadow');" +
            "darkLayer.parentNode.removeChild(darkLayer);} shadowDisable();";

        #endregion

        public static string SerealizeObjectToJsonString(string functionName, params object[] parameters)
        {
            string script = functionName + "('";
            for (int i = 0; i < parameters.Length; i++)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                script += JsonSerializer.Serialize(parameters[i]);
                //script += JsonConvert.SerializeObject(parameters[i]);
                if (i < parameters.Length - 1)
                {
                    script += ", ";
                }
            }
            script += "');";

            return script;
        }

        public static string SerealizeObject(string functionName, params object[] parameters)
        {
            string script = functionName + "(";
            for (int i = 0; i < parameters.Length; i++)
            {

                var options = new JsonSerializerOptions { WriteIndented = true };
                script += JsonSerializer.Serialize(parameters[i]);
                //script += JsonConvert.SerializeObject(parameters[i]);
                if (i < parameters.Length - 1)
                {
                    script += ", ";
                }
            }
            script += ");";

            return script;
        }

    }
}
