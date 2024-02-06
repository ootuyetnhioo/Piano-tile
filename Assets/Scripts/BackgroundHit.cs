using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundHit : MonoBehaviour
{
    public GameManager gm;
    public AudioSource offtune;

    // Phương thức được gọi khi người chơi nhấp chuột vào nền
    private void OnMouseDown()
    {
        // Chơi âm thanh khi người chơi nhấp chuột vào nền
        offtune.Play();
        // Kết thúc trò chơi bằng cách gọi phương thức EndGame() từ GameManager
        gm.EndGame();
    }
}
