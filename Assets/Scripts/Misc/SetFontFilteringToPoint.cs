using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SetFontFilteringToPoint : MonoBehaviour
{
  //http://forum.unity3d.com/threads/new-ui-text-is-useless-for-scaled-pixel-art-games.264894/#post-2084776
  void Start ()
  {
    GetComponent<Text>().font.material.mainTexture.filterMode = FilterMode.Point;
  }
}
