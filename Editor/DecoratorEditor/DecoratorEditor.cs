using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Shoot.Editor
{
    public abstract class DecoratorEditor : UnityEditor.Editor
    {
        private Type editorType;
        private UnityEditor.Editor editorInstance;

        protected UnityEditor.Editor EditorInstance
        {
            get
            {
                if (editorInstance == null && target != null && targets.Length > 0)
                {
                    editorInstance = CreateEditor(targets, editorType);
                }

                return editorInstance;
            }
        }

        public DecoratorEditor(string editorTypeName)
        {
            var editorAssembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            editorType = editorAssembly.GetType($"UnityEditor.{editorTypeName}");
            CheckCustomEditorType();
        }

        private void CheckCustomEditorType()
        {
            if (editorType == null)
            {
                throw new ArgumentException($"Editor {editorType} doesn't exist !!!");
            }
            var editedObjType = GetCustomEditorType(GetType());
            var targetEditedObjType = GetCustomEditorType(editorType);
            if (editedObjType != targetEditedObjType)
            {
                throw new ArgumentException($"Type {editedObjType} doesn't match the editor {editorType.Name} !!!");
            }
        }

        private Type GetCustomEditorType(Type type)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var attributes = type.GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
            var field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType", flags)).First();
            return field.GetValue(attributes[0]) as Type;
        }

        private void OnDisable()
        {
            if (editorInstance != null)
            {
                DestroyImmediate(editorInstance);
            }
        }

        /// <summary>
        /// 替换基类Editor的OnInspectorGUI方法
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorInstance.OnInspectorGUI();
        }
        
        // TODO：更多覆盖方法可在以下补充
    }
}