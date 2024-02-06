using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MusicNotes musicNotes;
    private string[] notes;
    public AudioSource[] notesSound;

    // Các thuộc tính cho khoảng cách giữa các dòng và cột ghi chú, tùy chỉnh thông số tile.
    [Header("Steps")]
    public float stepY = 2.7f;
    public float stepX = 1.786f;

    // Các thuộc tính cho khởi tạo tile và cài đặt vị trí xuất phát của chúng.
    [Header("Tile Settings")]
    public Vector2 startPoint;
    public GameObject noteTile;
    public float speed = 1;
    private float currentY;
    public float screenYLimit = -6.5f;
    private int score = 0;
    private int remainingNotes = 0;

    // Các thuộc tính liên quan đến hiển thị điểm và thanh trạng thái.
    [Header("Score")]
    public Text scoreText;
    public Slider remainingNotesSlider;
    private int clickedTiles = 0;

    // Các thuộc tính liên quan đến các menu trong trò chơi.
    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject endMenu;
    public Text endScore;

    // Biến kiểm tra trạng thái game đang pause hay không.
    public bool isPaused = true;

    public void Start()
    {
        // Khởi tạo số lượng ghi chú còn lại và lấy thông tin từ script MusicNotes.
        remainingNotes = musicNotes.notes.Length;
        notes = musicNotes.notes;
        isPaused = true;

        // Đặt vị trí bắt đầu của dòng ghi chú.
        currentY = startPoint.y;

        // Khởi tạo các ghi chú ban đầu.
        InitiateTiles();
    }

    // Phương thức để khởi tạo các nốt nhạc ban đầu.
    public void InitiateTiles()
    {
        // Tạo một đối tượng cha để chứa các nốt nhạc.
        GameObject notesParent = new GameObject("NotesParent");

        // Duyệt qua mảng nốt nhạc và tạo các tile tương ứng.
        for (int i = 0; i < notes.Length; i++)
        {
            // Tách tên nốt nhạc và thứ tự từ chuỗi nốt nhạc.
            string n = notes[i].Split(':')[0];
            int order = int.Parse(notes[i].Split(':')[1]);

            // Tính toán vị trí của tile dựa trên thứ tự của nốt nhạc.
            Vector2 _tmpPoint = startPoint;
            _tmpPoint.x += stepX * (order - 1);
            _tmpPoint.y = currentY;

            // Tạo tile và thiết lập các thuộc tính của nó.
            GameObject tmp_note = Instantiate(noteTile, _tmpPoint, Quaternion.identity, notesParent.transform);

            tmp_note.GetComponent<Note>().note = n;
            tmp_note.GetComponent<Note>().gm = this;

            // Cập nhật vị trí Y cho dòng tiếp theo với khoảng cách ngẫu nhiên.
            currentY += stepY * Random.Range(1, 3);
        }
    }

    // Phương thức để phát âm thanh khi người chơi nhấn vào một nốt nhạc.
    public void PlayNote(string note)
    {
        // Phát âm thanh tương ứng với nốt nhạc.
        switch (note)
        {
            case "A":
                notesSound[0].Play();
                break;
            case "B":
                notesSound[1].Play();
                break;
            case "C":
                notesSound[2].Play();
                break;
            case "D":
                notesSound[3].Play();
                break;
            case "E":
                notesSound[4].Play();
                break;
            case "F":
                notesSound[5].Play();
                break;
            case "G":
                notesSound[6].Play();
                break;
        }

        // Cập nhật điểm, số lượng nốt nhạc còn lại, và thanh trạng thái.
        score += 25;
        scoreText.text = score.ToString();
        remainingNotes--;
        remainingNotesSlider.value = Mathf.Clamp01(remainingNotes / (float)musicNotes.notes.Length);

        // Tăng biến đếm số lượng nốt nhạc đã nhấn.
        clickedTiles++;

        // Nếu đã nhấn hết tất cả ghi chú, kết thúc trò chơi.
        if (clickedTiles == notes.Length)
            EndGame();
    }

    // Phương thức để kết thúc trò chơi.
    public void EndGame()
    {
        speed = 0;
        isPaused = true;
        StartCoroutine(ShowEndMenu());
    }

    // Phương thức để hiển thị menu kết thúc sau một khoảng thời gian chờ.
    private IEnumerator ShowEndMenu()
    {
        yield return new WaitForSeconds(1);
        endMenu.SetActive(true);
        endScore.text = score.ToString();
    }

    // Phương thức để tạm dừng và tiếp tục trò chơi.
    public void Pause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
    }

    // Phương thức để khởi động lại trò chơi.
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
