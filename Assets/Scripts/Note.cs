using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    // Biến để xác định xem đối tượng là một ô bắt đầu hay không
    public bool startTile = false;

    // Tham chiếu đến GameManager để có thể giao tiếp với trò chơi
    public GameManager gm;

    // Biến để lưu trữ thông tin về nút âm thanh của đối tượng
    [HideInInspector]
    public string note;

    // Biến để xác định giới hạn y của màn hình
    private float limit;

    // Biến để kiểm tra xem đối tượng có đang hoạt động hay không
    private bool active;

    // Biến để lưu trữ tham chiếu đến SpriteRenderer để có thể thay đổi alpha và màu sắc
    private SpriteRenderer spriteRenderer;

    // Phương thức Awake được gọi khi đối tượng được khởi tạo
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        active = true; // Đối tượng mặc định là hoạt động khi được khởi tạo
    }

    // Phương thức Start được gọi khi đối tượng được khởi tạo và trò chơi bắt đầu
    private void Start()
    {
        limit = gm.screenYLimit;

        // Nếu đối tượng là ô bắt đầu, di chuyển nó ngẫu nhiên theo trục X
        if (startTile)
        {
            Vector2 randomPos = transform.position;
            randomPos.x += gm.stepX * Random.Range(0, 4);
            transform.position = randomPos;
        }
    }

    // Phương thức LateUpdate được gọi sau khi tất cả các phương thức Update được gọi trong frame hiện tại
    private void LateUpdate()
    {
        // Nếu trò chơi đang tạm dừng, không thực hiện các thay đổi vị trí và kiểm tra kết thúc trò chơi
        if (gm.isPaused)
            return;

        // Di chuyển đối tượng theo hướng từ trên xuống dưới với tốc độ được quy định bởi GameManager
        transform.Translate(new Vector3(0, -gm.speed * Time.deltaTime, 0));

        // Kiểm tra nếu đối tượng vượt quá giới hạn Y, kết thúc trò chơi
        if (active && transform.position.y < limit)
            gm.EndGame();
    }

    // Phương thức được gọi khi người chơi nhấp chuột vào đối tượng
    private void OnMouseDown()
    {
        // Nếu đối tượng đang hoạt động và trò chơi không tạm dừng
        if (active && !gm.isPaused)
        {
            // Chơi âm thanh tương ứng với đối tượng và bắt đầu quá trình làm mờ và biến mất
            gm.PlayNote(note);
            StartCoroutine(FadeOutAndDisappear());
            active = false; // Đánh dấu là đối tượng không còn hoạt động
        }

        // Nếu đối tượng là ô bắt đầu, cũng bắt đầu quá trình làm mờ và biến mất và tiếp tục trò chơi
        if (startTile)
        {
            StartCoroutine(FadeOutAndDisappear());
            active = false; // Đánh dấu là đối tượng không còn hoạt động
            gm.isPaused = false; // Tiếp tục trò chơi
        }
    }

    // Coroutine để làm mờ và biến mất đối tượng
    IEnumerator FadeOutAndDisappear()
    {
        float duration = 1.0f;
        float elapsedTime = 0f;

        // Thực hiện làm mờ và biến mất trong khoảng thời gian duration
        while (elapsedTime < duration)
        {
            float scale = Mathf.Lerp(1f, 1.5f, Mathf.Pow(elapsedTime / duration, 2f));
            transform.localScale = new Vector3(scale, scale, 1f);

            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            SetAlpha(alpha);

            elapsedTime += Time.deltaTime;
            yield return null; // Chờ một frame
        }

        SetAlpha(0f); // Đặt alpha về 0 để đảm bảo đối tượng hoàn toàn biến mất
    }

    // Phương thức để thiết lập alpha cho SpriteRenderer
    void SetAlpha(float alpha)
    {
        Color currentColor = spriteRenderer.color;
        currentColor.a = alpha;
        spriteRenderer.color = currentColor;
    }

    // Phương thức để xóa đối tượng khỏi scene
    public void Remove()
    {
        Destroy(gameObject);
    }
}
