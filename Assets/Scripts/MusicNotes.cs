using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MusicNotes : ScriptableObject
{
    // Mảng chứa các chuỗi biểu diễn thông tin về các nốt nhạc. 
    // Định dạng: [Note:Order] (Ví dụ: A:3)
    [Header("Format: [Note:Order] (Ex: A:3)")]
    public string[] notes;

    // Hàm sẽ được gọi khi người dùng chọn "Randomize" từ trình đơn ngữ cảnh.
    [ContextMenu("Randomize")]
    public void Randomize()
    {
        // Tạo một mảng mới với số lượng phần tử ngẫu nhiên từ 25 đến 50.
        notes = new string[Random.Range(25, 50)];
        
        // Mảng chứa các chữ cái đại diện cho các nốt nhạc cơ bản.
        string[] letters =
        {
            "A", "B", "C", "D", "E", "F", "G"
        };

        // Lặp qua từng phần tử trong mảng notes để gán giá trị ngẫu nhiên.
        for (int i = 0; i < notes.Length; i++)
            notes[i] = letters[Random.Range(0, 7)] + ":" + Random.Range(1, 5);
    }
}
