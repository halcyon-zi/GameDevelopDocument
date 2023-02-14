# UI制作说明

### 序、UI框架介绍

    该部分为项目组内UI框架的简短介绍，主要介绍了UI框架的**组成部分**与**工作流程**，不感兴趣的话该部分可以忽略不看。

###### 组成部分

    项目组内的UI框架大致与中台的UI框架无异，仅在部分地方做过一些修改。UI框架采用**C#与lua**进行编写，其中C#处理与**UGUI**紧密相连的逻辑，而lua则处理**上层调用**的逻辑，并且UI常用的操作在C#与lua中都编写了相应的接口，以此来降低lua与C#的交互次数，提高性能。

###### 工作流程

    以下流程，括号的内容表示该步骤在**哪个环境**下执行。

- 在lua代码中调用**UIManager.GetUI**来打开**UIMap**中的UI界面（lua）

- 调用父类**UIBaseWindow**构造函数（lua）

- 调用**UIUtils**的生成函数，跳转至**UIFactory**生成UI窗口（c#）

- 为当前UI生成**UIState**来记录信息，同时实例化UI，并且绑定**UIController**（c#）

- UIController调用lua代码的**OnInitEnd**与**OnCtorEnd**，并将UIController与UI的lua引用进行绑定（lua）

- **UI加载结束**

- UI界面中的**各组件**通过父类**UIBaseCom**获取自身UGUI上的组件引用，在lua层进行调用（lua）

### 一、组件介绍

    该部分将介绍一些常用的UI控件(组件)及其配置方法。

##### 1.1 Panel

    UGUI中最基础的控件之一，尺寸与**Canvas**一样大，仅有**Image**控件。

<img title="" src="file:///img/Panel.png" alt="" data-align="center" width="254">


    Panel多作为模块与模块之间的划分，如果你需要划分两个功能模块，例如技能按钮区与移动摇杆区，则可以将不同功能的UI放置在不同的Panel下。Image控件可以根据需求考虑是否保留。

##### 1.2 Image

    UGUI最基础的控件之一，具有**Image**控件。

<img title="" src="file:///img/Image.png" alt="" data-align="center" width="268">

- SourceImage可以选择自己需要显示的图片

- ImageType可以选择图片的展示模式，如果需要图片实现**填充效果**，需要将ImageType选为**Fill模式**

- RaycastTarget，如果此图片不需要相应点击事件，可以取消勾选此选项

- Maskable，如果此图片不需要被遮罩，可以取消勾选此选项

##### 1.3 Button

    UGUI最基础的控件之一，具有**Button**、**Image**和**Text**控件。

   ![](img\Button1.png)    ![](img\Button2.png)

- Image为Button上的图案，由于Button需要相应点击事件，因此RaycastTarget一般都勾选上

- Button则为按钮身上的控件

- Text为Button上显示的文字，不需要文字时可以删除

##### 1.4 InputField

    UGUI中的**输入栏控件**，具有**Image**、**InputField**和两个**Text**

<img src="file:///img/Input1.png" title="" alt="Input1.png" data-align="center">

<img src="file:///img/Input2.png" title="" alt="Input2.png" data-align="center">

- Image为输入栏的背景图片

- InputField为本身控件，ReadOnly勾选后可以让此输入栏只读

- Placeholder的Text为输入框默认展示文字

- Text的Text为输入框实际输入文字

##### 1.5 Text & TextMeshPro

    Text是UGUI中基本的**文本控件**，而TextMeshPro则为文本控件的Pro版本，可以实现Text所不能实现的更多效果，例如阴影、描边，不需要特殊效果的文字建议采用Text就足够，可以提升性能。

    ![](img\Text.png)    ![](img\TextMeshPro.png)

- Text中的RaycastTarget与Maskable与Image的一样，不需要响应点击事件与遮罩时可以取消勾选

- 勾选RichText，可以支持富文本，实现部分文字内容定义颜色、字体、大小等

##### 1.6 Toggle

    UGUI中的**勾选框控件**，具有一个**背景框**，一个**已勾选图片**和**一段描述**

<img src="file:///img/Toggle.png" title="" alt="" data-align="center">

- isOn为此时的勾选状态

##### 1.7 Dropdown

    UGUI中的**下拉选单控件**。

<img src="file:///img/Dropdown.png" title="" alt="" data-align="center">

- Options为下拉选单的选项，可以自定义设置若干选项，每个选项对应的Sprite则为该选项的背景图片

##### 1.8 Slider

    UGUI中的**滑动器控件**，可以滑动中间的**浮块**来确定当前的数值。

<img src="file:///img/Slider.png" title="" alt="" data-align="center">

- Direction为浮块的滑动方向

- MinValue与MaxValue代表该控件的取值范围

- Value为当前值

##### 1.9 ScrollView

    UGUI中的**滚动列表控件**，由**一个Panel**和**两个滚动条**组成，是较为常用的一种控件。

<img src="file:///img/ScrollView.png" title="" alt="" data-align="center">

- HorizontalScrollbar、VerticalScrollbar分别为**水平滚动条**与**竖直滚动条**，不需要滚动条的话可以删除

- MovementType为拖拽模式，分别为无弹性的、弹性的、截断的

- ScrollSensitivity为滚动灵敏度

- Horizontal、Vertical两个勾选框分别表示能否对该方向进行拖拽

### 二、命名规范

    该部分主要介绍如何为UI以及其子节点命名，命名将采取**大驼峰**命名。

<img title="" src="file:///img/Name.png" alt="" data-align="center">

##### 2.1 窗口UI

    UI名 = “功能描述” + UI。例：角色界面，UI：RoleUI

##### 2.2 子UI

    UI名 = “功能描述” + UI 或 “实体名” + Item/Entry。例：每个子UI表示一个角色的头像，UI：RoleProfileEntry

##### 2.3 控件

    UI名 = “控件名简写” +  “功能描述”。例：角色名，UI：TxtRoleName

##### 2.4 其他

    仅仅只是作为其他UI容器的Obj，尽量使用功能描述作为自己的UI名

### 三、自适应

<img src="file:///img/RectTransform.png" title="" alt="" data-align="center">

    在Canvas下的UI，Transform组件会变成**RectTransform**，而RectTransform中的**Anchors**和**Pivot**是影响自适应的关键属性。

##### 3.1 Anchors（锚点）

<img src="file:///img/Anchor.png" title="" alt="" data-align="center">

    四个锚点确定在父UI上，无论父UI如何变化，子UI**四个顶点距离四个锚点的距离保持不变**，由此可以确定子UI希望位于父UI的哪个位置。

    Unity种预设了16种锚点的位置，分别对应左上角、正上方、右上角、随上方拉伸等

##### 3.2 Pivot（轴心）

<img src="file:///img/Pivot.png" title="" alt="" data-align="center">

    是UI本身的**中心点**，UI的**旋转**与**缩放**均会基于这个点。

    在设置锚点时，按下Shift同时可以对轴心进行设置。

### 四、布局与遮罩

#### 4.1 布局

    使用布局系统可以根据元素包含的内容**自动调整元素的大小与位置**。

- HorizontalLayoutGroup：
  
  <img title="" src="file:///img/HorizontalLayoutGroup.png" alt="" data-align="center">
  
  **水平布局组组件将其子布局元素并排放置在一起。**
  
  | 属性                                                                                  | 功能                               |
  | ----------------------------------------------------------------------------------- | -------------------------------- |
  | Child Alignment                                                                     | 用于子布局元素的对齐方式。                    |
  | Control Child Size                                                                  | 布局组是否控制其子布局元素的宽度和高度。             |
  | Use Child Scale                                                                     | 在为元素调整大小和进行布局时，布局组是否考虑其子布局元素的缩放。 |
  | **Width** 和 **Height** 对应于每个子布局元素的 Rect Transform 组件中的 **Scale.X** 和 **Scale.Y** 值。 |                                  |
  | Child Force Expand                                                                  | 是否要强制子布局元素扩展以填充额外的可用空间。          |

- VerticalLayoutGroup：
  
  <img title="" src="file:///img/VerticalLayoutGroup.png" alt="" data-align="center">
  
  **垂直布局组组件将子布局元素纵向放置。**
  
  属性与HorizontalLayoutGroup一致。

- GridLayoutGroup：
  
  <img src="file:///img/GridLayoutGroup.png" title="" alt="" data-align="center">
  
  **网格布局组组件将其子布局元素放在网格中。**
  
  | 属性              | 功能                                                       |
  | --------------- | -------------------------------------------------------- |
  | Cell Size       | 要用于组内每个布局元素的大小。                                          |
  | Start Corner    | 第一个元素所在的角。                                               |
  | Start Axis      | 沿哪个主轴放置元素。Horizontal 将在填充整行后才开始新行。Vertical 将在填充整列后才开始新列。 |
  | Child Alignment | 用于布局元素的对齐方式。                                             |
  | Constraint      | 将网格约束为固定数量的行或列以便支持自动布局系统。                                |

- ContentSizeFitter
  
  <img src="file:///img/ContentSizeFitter.png" title="" alt="" data-align="center">
  
  **内容大小适配器**充当布局控制器，可用于控制其自身布局元素的大小。查看实际自动布局系统的最简单方法是向带有文本组件的游戏对象添加内容大小适配器组件。
  
  值得注意的是，当调整矩形变换的大小时（无论是通过内容大小适配器还是其他工具），大小调整是围绕轴心进行的。这意味着可使用轴心来控制大小调整的方向。
  
  例如，当轴心位于中心位置时，内容大小适配器将在所有方向朝外均匀扩展矩形变换。当轴心位于左上角时，内容大小适配器将向右下方向扩展矩形变换。

- LayoutElement
  
  <img src="file:///img/LayoutElement.png" title="" alt="" data-align="center">
  
  如果要**覆盖**最小大小、偏好大小或灵活大小，可通过向游戏对象添加**布局元素组件**来实现。
  
  使用布局元素组件可以覆盖一个或多个布局属性的值。启用要覆盖的属性的复选框，然后指定要用于覆盖的值。
  
  **布局控制器**按以下顺序为布局元素分配宽度或高度：
  
  - 首先，布局控制器将分配**最小大小**属性（**Min Width**、**Min Height**）。
  - 如果有足够的可用空间，布局控制器将分配**偏好大小**属性（**Preferred Width**、**Preferred Height**）。
  - 如果有额外的可用空间，布局控制器将分配**灵活大小**属性（**Flexible Width**、**Flexible Height**）。

#### 4.2 遮罩

- Mask
  
  <img src="file:///img/Mask.png" title="" alt="Mask.png" data-align="center">
  
  **Mask**不是可见的 UI 控件，而是一种修改控件子元素外观的方法。遮罩将子元素限制（即“掩盖”）为父元素的形状。因此，如果子项比父项大，则子项仅包含在父项以内的部分才可见。
  
  ShowGraphic：是否应在子对象上使用Alpha绘制遮罩（父）对象的图像。

- RectMask2D
  
  <img src="file:///img/RectMask2D.png" title="" alt="" data-align="center">
  
  RectMask2D 的一个常见用途是显示较大区域的小部分。使用 RectMask2D 可框定此区域。
  
  RectMask2D 控件的局限性包括：
  
  - 仅在 2D 空间中有效
  - 不能正确掩盖不共面的元素

### 五、通用UI与子UI

#### 5.1 通用UI(CommonUI)

    制作UI的过程中，难免会遇到同一个**UI反复利用**的情况，例如各种按钮，各种确认窗口，各种道具图标等等，可以将这些UI做成一个**通用UI预制体**，以便在各种UI界面可以直接拖入使用，既能简化操作，又能保证UI风格统一。

<img src="file:///img/CommonUI.png" title="" alt="" data-align="center">

#### 5.2 子UI(ChildUI)

    在一个UI界面中，你可能需要若干个相似的组件来完成一些需求，例如角色列表展示，道具列表展示，在制作UI的过程中并不确定有多少角色以及多少道具，但是可以确定的是每一个角色UI、道具UI均保持一致，此时就需要将角色UI或者道具UI做成一个子UI，而具体生成多少个子UI，则由逻辑代码生成。

    子UI完成制作后，大部分情况都需要将子UI放置在一些布局内，例如布置在VerticalLayoutGroup中，因此可以先将做好的一个子UI拖入其中，复制多个来调整布局查看效果，之后再删除。

<img src="file:///img/ChildUI.png" title="" alt="" data-align="center">

### 六、多语言配置

    多语言配置，即一个显示文字的UI，需要根据不同语言版本，去展示不同语言文字，其中包括静态多语言、动态多语言，其中每个又分为多语言文字、多语言图片。但无论是哪种多语言，均需要在多语言表（Language-多语言.xlsx）中进行配置。

#### 6.1 静态多语言

    静态多语言，指这个UI无论如何只会显示这一种文字或者图片，例如菜单里的一些文字，不会根据其他因素而改变内容，因此需要配置静态多语言。

    首先需要在其根UI上添加【UIStaticLangReplacer】组件，然后将需要显示多语言的Text或者Image拖入相应位置，填写配置的多语言主键即可。

<img src="file:///img/Language.png" title="" alt="" data-align="center">

#### 6.2 动态多语言

    动态多语言，则需要变更展示内容的多语言，例如玩家当前多少级，需要多少数量的材料等，需要有部分内容是动态变更的，这种多语言在配置的时候，需要将动态变更的部分用{x}表示出来，x表示第几个参数填入此空，x从0开始计数。

<img title="" src="file:///img/Language2.png" alt="" data-align="center">

### 七、优化相关

- 使用Mask会产生**额外**2个drawCall，但**Mask之间可以合批**；使用RectMask2D不产生额外drawCall，但**RectMask2D之间不能合批**。其内部UI均**不能与外部UI进行合批**。
- **不要**使用Unity的**Outline**与**Shadow**组件，改用**TextMeshPro**实现这两种功能。
- **不需要接收点击事件**的UI把**RayCastTarget关闭**，以减少事件响应。
- 频繁需要隐藏的UI，建议使**用CanvasGroup**组件，将UI**透明**，少用Disable与Enable，否则Canvas将会重建。
- 可以合批的UI中间不要放置中间层，将其他UI尽量置于合批UI的下方。
- 动静分离，**将静态UI与动态UI分离到不同的Canvas**中。
- 当有覆盖全屏的UI时，可以关闭不可见的摄像机，例如游戏场景的摄像机。

### 八、资源路径

#### 8.1 UI预制体路径

**<font color=red>Assets/GameResources/UI/Prefab</font>**

创建新UI时，建立一个新文件夹放进去，跟其相关的一些子UI可以放置在内。

#### 8.2 UI图片路径

静态的**不会更替**的图片路径：**<font color=red>Assets/LocalResources/UI/Texture</font>**

动态的**可能会变更**的图片路径：**<font color=red>Assets/GameResources/UI/Texture</font>**

放置完图片后，记得重新打一下图集，对更新后的文件夹**右键->Tools->GenerateAtlas(UI) Recursively**。

<img title="" src="file:///img/AtlasUI.png" alt="" data-align="center">

#### 8.3 UI动画路径

**<font color=red>Assets/LocalResources/UI/Animation</font>**

#### 8.4 UI材质路径

**<font color=red>Assets/LocalResources/UI/Material</font>**
