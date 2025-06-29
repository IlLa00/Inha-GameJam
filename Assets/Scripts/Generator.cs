using UnityEngine;
using UnityEngine.UI;

public class Generator : InteractiveObject
{
    private float MaxProgress = 1f;
    private float CurrentProgress = 0f;
    private float SuccessProgress = 0.1f;
    private float FailProgress = 0.05f;

    private float GameTimer = 0f;
    private float RandomGameStart;

    private float Min_Range;
    private float Max_Range;

    private bool IsCompleting = false;
    private bool IsRepairing = false;
    private bool IsPlayingGame = false;

    public float GetCurrentProgress() { return CurrentProgress; }
    public float GetMinRange() { return Min_Range; }
    public float GetMaxRange() { return Max_Range; }

    public bool IsComplete() { return IsCompleting; }
    public bool IsRepair() { return IsRepairing; }
    public bool IsPlayGame() { return IsPlayingGame; }

    void Start()
    {
        
    }

    protected override void Update()
    {
        if (IsComplete()) return;

        base.Update();

        if (CurrentProgress >= MaxProgress) // 맥스 게이지에 도달하면 완료 처리
        {
            IsCompleting = true;
            IsRepairing = false;
            return;
        }

        if (!IsComplete() && IsRepairing && !IsPlayingGame && CurrentProgress < MaxProgress)
        {
            // Debug.Log("Generateor OnInteracting");

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;

            // 발전기 게이지 상승
            CurrentProgress += 0.05f * Time.deltaTime;
            CurrentProgress = Mathf.Clamp(CurrentProgress, 0f, MaxProgress);
            // Debug.Log(CurrentProgress);

            // Debug.Log(RandomGameStart);
            
            GameTimer += Time.deltaTime;
            if (GameTimer > RandomGameStart)
            {
                // Debug.Log("!!");
                StartMiniGame();
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) // 이동키 입력 시, 탈출
            {
                IsRepairing = false;
                IsPlayingGame = false;
            }
        }

        if(!IsRepair())
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.white;
        }
    }

    public override void OnInteractive()
    {
        if (IsInteracting) return;
        if (IsComplete())
        {
            Debug.Log("끝난 발전기!!");
            return; // 성공한거면 안함.
        }

        Debug.Log("Starting Generateor OnInteractive");

        // 발전기 시작
        IsRepairing = true;

        RandomGameStart = Random.Range(0, 10);
    }

    private void StartMiniGame()
    {
        Debug.Log("Starting Generateor Minigame");

        if(IsPlayingGame == false)
        {
            IsPlayingGame = true;
            Max_Range = Random.Range(0.2f, 1f);
            Min_Range = Max_Range - 0.2f;
        }
       
    }

    public void SuccessMiniGame()
    {
        CurrentProgress += SuccessProgress;
        Debug.Log(CurrentProgress);
        StopMiniGame();
    }

    public void FailMiniGame()
    {
        // 게임센터에 큰 소리
        Player.IncreaseCurrentNoiseLevel(20);

        CurrentProgress -= FailProgress;
        Debug.Log(CurrentProgress);
        StopMiniGame();
    }

    private void StopMiniGame()
    {
        IsPlayingGame = false;

        RandomGameStart = Random.Range(0, 10);
    }
}
