using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventBool : UnityEvent<bool> { }

[System.Serializable]
public class UnityEventTransform : UnityEvent<Transform> { }

[System.Serializable]
public class UnityEventGameObject : UnityEvent<GameObject> { }

[System.Serializable]
public class UnityEventInt : UnityEvent<int> { }