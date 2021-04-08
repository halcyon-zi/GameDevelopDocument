### Entitas 概要

#### 一、 ECS框架简介

**ECS，实体-组件-系统。**

- E：Entity，实体。游戏内每个基本单元都是一个**实体**，每个实体由一个或多个**组件**构成。
- C：Component，组件。组件中仅包含**数据**，不包含任何**方法**。
- S：System，系统。系统**处理**拥有一个或多个组件的实体集合，只包含**行为**，不包含任何**数据**。

#### 二、Entitas结构

Entitas作为一个ECS框架，其也包含了Entity、Component与System，不仅如此，它还包含了Context、Matcher、Group与Collector。

- Entity：实体，Component的载体，当自身的Component发生变化时会派发事件，更新Group，Context中的相关集合。
- Component：组件，只包含数据。
- System：系统，只包含行为，进行逻辑处理。
    - Systems：System管理类，所有System都需要添加到Systems实例中进行管理。
    - ISystem：四种类型的系统
        - IInitializeSystem：初始化系统
        - IExecuteSystem：帧执行系统
            - ReactiveSystem：响应系统，通过collector收集发生改变同时满足筛选的Entity集合，统一进行逻辑处理
            - MultiReactiveSystem：多容器响应系统，功能与上面类似
            - JobSystem：多线程系统，在指定线程中执行Execute
        - ICleanupSystem：清理系统，所有逻辑系统更新完后每帧执行
        - ITearDownSystem：卸载系统，Entitas系统结束前执行一次
- Context：上下文环境，可以同时存在多个Context，统一通过Contexts实例获取与管理。每个Context均会管理当前环境的Entity与Group。
- Group：组，满足一类Matcher的Entity集合。Group由Context创建并缓存，当Entity的Component发生改变时，会更新Group中的Entity集合。
- Matcher：匹配器，检测Entity身上的Component是否满足匹配条件。
- Collector：收集器，在Context观察Group，收集Group中变化的Entity。

#### 三、Entitas运行流程

**GameController**会持有当前**上下文环境Context**与**系统Systems**。

**Context**持有**Entity**与**Group**。

**System**管理四种类型的系统，其中IExecuteSystem会每帧响应。

当**Entity**上的**Component**发生变化时（增/删/改），所有的修改会通过**Context**通知所有**Group**，Group通过**Matcher**判断是否需要更新自己。Group更新后，会将添加或删除事件转发出去，**Collector**会监听事件，并收集对应的Entity。此时**ReactiveSystem响应系统**每帧都在Exectue中对收集到的Entity通过filter筛选满足条件的Entity，最后交予_execute进行处理。