#### “装饰” Editor

##### 一、拓展Editor

​	编写Editor脚本放入Editor目录下，利用`[CustomEditor(typeof(***Editor))]`特性去自定义Editor，继承Editor，覆盖一些列Editor生命周期函数，实现自己需要的功能。

​	例如：

``` c#
[CustomEditor(typeof(RectTransform))]
public class MyTest : Editor 
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		if(GUILayout.Button("Adding this button"))
		{
			Debug.Log("Adding this button");
		}
	}
}
```

##### 二、拓展未暴露的Editor并不影响原有布局

​    直接继承自Editor实现自定义Editor有时会影响原先布局，原因在于需要拓展的Editor有自己的Editor类脚本，需要继承这些Editor，例如RectTransform的Editor是RectTransformEditor，ParticleSystem的Editor是ParticleSystemInspector。

​	但这些Editor并不是一个对外公开的类，所以不能继承它。

​	因此需要使用反射的方法来解决。

**DecoratorEditor**

    ```c#
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
    ```

**Editor**

```c#
[CustomEditor(typeof(RectTransform))]
public class MyTest : DecoratorEditor
{
	public MyTest(): base("RectTransformEditor"){}
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		if(GUILayout.Button("Adding this button"))
		{
			Debug.Log("Adding this button");
		}
	}
}
```



