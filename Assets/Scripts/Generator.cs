using UnityEngine;

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

    private bool IsComplete = false;
    private bool IsRepairing = false;
    private bool IsPlayingGame = false;

    public float GetCurrentProgress() { return CurrentProgress; }
    public float GetMinRange() { return Min_Range; }
    public float GetMaxRange() { return Max_Range; }
    public bool IsRepair() { return IsRepairing; }
    public bool IsPlayGame() { return IsPlayingGame; }

    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();

        if (!IsComplete && IsRepairing && !IsPlayingGame && CurrentProgress < MaxProgress)
        {
            if (CurrentProgress >= MaxProgress) // 맥스 게이지에 도달하면 완료 처리
            {
                IsComplete = true;
                IsRepairing = false;
            }

            // 발전기 게이지 상승
            CurrentProgress += 0.01f * Time.deltaTime;
            CurrentProgress = Mathf.Clamp(CurrentProgress, 0f, MaxProgress);

            GameTimer += Time.deltaTime;
            if (GameTimer > RandomGameStart)
                StartMiniGame();

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) // 이동키 입력 시, 탈출
            {
                IsRepairing = false;
                IsPlayingGame = false;
            }

        }

        if(IsPlayingGame)
        {

        }

    }

    protected override void OnInteractive()
    {
        if (IsComplete) return; // 성공한거면 안함.

        Debug.Log("Starting Generateor OnInteractive");

        // 발전기 시작
        IsRepairing = true;

        RandomGameStart = Random.Range(0, 10);
    }

    private void StartMiniGame()
    {
        Debug.Log("Starting Generateor Minigame");

        Max_Range = Random.Range(0.2f, 1f);
        Min_Range = Max_Range - 0.2f;
    }

    public void SuccessMiniGame()
    {
        CurrentProgress += SuccessProgress;
    }

    public void FailMiniGame()
    {
        CurrentProgress -= FailProgress;
    }

    private void StopMiniGame()
    {
        IsPlayingGame = false;

        RandomGameStart = Random.Range(0, 10);
    }
}
