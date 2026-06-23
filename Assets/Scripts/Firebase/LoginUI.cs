using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject nicknamePanel;

    [Header("Login Form")]
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button signupButton;
    [SerializeField] private TextMeshProUGUI errorText;

    [Header("Nickname Form")]
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI nicknameErrorText;

    private string pendingEmail;
    private string pendingPassword;

    private async UniTaskVoid Start()
    {
        await UniTask.WaitUntil(() => AuthManager.Instance.IsInitialized);
        await UniTask.WaitUntil(() => ProfileManager.Instance.IsInitialized);

        loginButton.onClick.AddListener(() => OnLoginClicked().Forget());
        signupButton.onClick.AddListener(() => OnSignupClicked().Forget());
        confirmButton.onClick.AddListener(() => OnConfirmNicknameClicked().Forget());

        AuthManager.Instance.LoginStateChanged += OnLoginStateChanged;

        UpdateUI();
    }

    private void UpdateUI()
    {
        bool isLoggedIn = AuthManager.Instance.IsLoggedIn;
        loginPanel.SetActive(!isLoggedIn);
        nicknamePanel.SetActive(false);
    }

    private void OnLoginStateChanged(bool signedIn)
    {
        UpdateUI();
    }

    private async UniTaskVoid OnLoginClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowError("이메일과 비밀번호를 입력하세요.");
            return;
        }

        SetButtonsInteractable(false);
        ClearError();

        var (success, error) = await AuthManager.Instance.SignInAsync(email, password);

        if (success)
        {
            loginPanel.SetActive(false);
        }
        else
        {
            ShowError(error);
        }

        SetButtonsInteractable(true);
    }

    private async UniTaskVoid OnSignupClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowError("이메일과 비밀번호를 입력하세요.");
            return;
        }

        SetButtonsInteractable(false);
        ClearError();

        var (success, error) = await AuthManager.Instance.CreateUserAsync(email, password);

        if (success)
        {
            // 회원가입 성공 → 닉네임 패널로 전환
            pendingEmail = email;
            pendingPassword = password;
            loginPanel.SetActive(false);
            nicknamePanel.SetActive(true);
        }
        else
        {
            ShowError(error);
        }

        SetButtonsInteractable(true);
    }

    private async UniTaskVoid OnConfirmNicknameClicked()
    {
        string nickname = nicknameInput.text.Trim();

        if (string.IsNullOrEmpty(nickname))
        {
            ShowNicknameError("닉네임을 입력하세요.");
            return;
        }

        if (nickname.Length < 2 || nickname.Length > 10)
        {
            ShowNicknameError("닉네임은 2자 이상 10자 이하로 입력하세요.");
            return;
        }

        confirmButton.interactable = false;
        ClearNicknameError();

        await UniTask.WaitUntil(() => AuthManager.Instance.IsLoggedIn);

        var (success, error) = await ProfileManager.Instance.SaveProfileAsync(nickname);

        if (success)
        {
            nicknamePanel.SetActive(false);
        }
        else
        {
            ShowNicknameError(error);
        }

        confirmButton.interactable = true;
    }

    private void ShowError(string message)
    {
        errorText.text = message;
    }

    private void ClearError()
    {
        errorText.text = string.Empty;
    }

    private void ShowNicknameError(string message)
    {
        nicknameErrorText.text = message;
    }

    private void ClearNicknameError()
    {
        nicknameErrorText.text = string.Empty;
    }

    private void SetButtonsInteractable(bool interactable)
    {
        loginButton.interactable = interactable;
        signupButton.interactable = interactable;
    }

    private void OnDestroy()
    {
        if (AuthManager.Instance != null)
            AuthManager.Instance.LoginStateChanged -= OnLoginStateChanged;
    }
}