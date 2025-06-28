using UnityEngine;

public class Generator : InteractiveObject
{
    private float MaxProgress = 100f;
    private float CurrentProgress = 0f;
    private float SuccessProgress = 10f;
    private float FailProgress = 5f;

    private float GameTimer = 0f;
    private float RandomGameStart;

    private float Min_Range;
    private float Max_Range;

    private bool IsComplete = false;
    private bool IsRepairing = false;
    private bool IsPlayGame = false;

    public float GetCurrentProgress() { return CurrentProgress; }
    public float GetMinRange() { return Min_Range; }
    public float GetMaxRange() { return Max_Range; }

    void Start()
    {
        
    }

    void Update()
    {
        if(!IsComplete && IsRepairing && !IsPlayGame && CurrentProgress < MaxProgress)
        {
            if (CurrentProgress >= MaxProgress) // 맥스 게이지에 도달하면 완료 처리
            {
                IsComplete = true;
                IsRepairing = false;
            }

            // 발전기 게이지 상승
            CurrentProgress += 0.1f * Time.deltaTime;
            CurrentProgress = Mathf.Clamp(CurrentProgress, 0f, MaxProgress);

            GameTimer += Time.deltaTime;
            if(GameTimer > RandomGameStart)
                StartMiniGame();
            
        }

        if(IsPlayGame)
        {

        }

    }

    protected override void OnInteractive()
    {
        // 부모에서 플레이어 널값 검사

        if (IsComplete) return; // 성공한거면 안함.

        Debug.Log("Starting Generateor OnInteractive");

        // 발전기 시작
        IsRepairing = true;

        RandomGameStart = Random.Range(0, 10);
    }

    private void StartMiniGame()
    {
        Debug.Log("Starting Generateor Minigame");

        Max_Range = Random.Range(20, 100);
        Min_Range = Max_Range - 20f;
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
        IsPlayGame = false;

        RandomGameStart = Random.Range(0, 10);
    }
}
