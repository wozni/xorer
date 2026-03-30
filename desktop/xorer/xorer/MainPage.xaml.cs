using CommunityToolkit.Maui.Storage;

namespace xorer
{
    public partial class MainPage : ContentPage
    {
        string KeyA;
        string KeyB;
        byte[] a;
        byte[] b;
        string result;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void openFromFileA_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(inputKeyA.Text) && inputKeyA.IsPassword == true)
            {
                KeyA = "";
                inputKeyA.Text = "";
            }
            var picker = await FilePicker.PickAsync(new PickOptions { });
            if (picker != null)
            {
                KeyA = File.ReadAllText(picker.FullPath).Trim();
                inputKeyA.Text = KeyA;
            }
        }

        private async void openFromFileB_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(inputKeyB.Text) && inputKeyB.IsPassword == true)
            {
                KeyB = "";
                inputKeyB.Text = "";
            }
            var picker = await FilePicker.PickAsync(new PickOptions { });
            if (picker != null)
            {
                KeyB = File.ReadAllText(picker.FullPath).Trim();
                inputKeyB.Text = KeyB;
            }
        }

        private void generateKey_Clicked(object sender, EventArgs e)
        {
            alertLabel.IsVisible = false;
            KeyA = inputKeyA.Text;
            KeyB = inputKeyB.Text;

            if (string.IsNullOrWhiteSpace(KeyA) || string.IsNullOrWhiteSpace(KeyB))
            {
                ShowError("Error: Missing key input!");
                return;
            }

            if (KeyA.Length != KeyB.Length)
            {
                ShowError("Error: Input lengths are not equal!");
                return;
            }

            if (!IsBase64(inputKeyA.Text) || (!IsBase64(inputKeyB.Text)))
            {
                ShowError("Error: Invalid Base64 format!");
                return;
            }

            a = Convert.FromBase64String(KeyA);
            b = Convert.FromBase64String(KeyB);

            byte[] xor = XOR(a, b);
            result = Convert.ToBase64String(xor);
            resultEntry.Text = result;
        }

        private async void copyTextButton_Clicked(object sender, EventArgs e)
        {
            string entry = resultEntry.Text;
            if (!string.IsNullOrWhiteSpace(entry))
            {
                await Clipboard.SetTextAsync(resultEntry.Text);
                alertLabel.IsVisible = true;
                alertLabel.Text = "Copied to clipboard!";
                alertLabel.TextColor = Colors.Black;
            }
        }

        private async void saveToFileButton_Clicked(object sender, EventArgs e)
        {
            string fileName = "result.txt";
            string keyText = result + "\n";

            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(keyText));

            var save = await FileSaver.Default.SaveAsync(fileName, stream);

            if (save.IsSuccessful)
            {
                alertLabel.IsVisible = true;
                alertLabel.Text = "Result has been saved to: " + Path.GetFileName(save.FilePath);
                alertLabel.TextColor = Colors.Black;
                return;
            }

            ShowError("Error: Saving failed!");
        }

        private void randomButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputKeyA.Text) && string.IsNullOrWhiteSpace(inputKeyB.Text))
            {
                GenerateRandomKey(inputKeyB, inputKeyA, 32);
                GenerateRandomKey(inputKeyA, inputKeyB, 32);
                return;
            }

            if (string.IsNullOrWhiteSpace(inputKeyA.Text))
            {
                GenerateRandomKey(inputKeyB, inputKeyA, 0);
                return;
            }

            if (string.IsNullOrWhiteSpace(inputKeyB.Text))
            {
                GenerateRandomKey(inputKeyA, inputKeyB, 0);
                return;
            }

            ShowError("Error: Both inputs are filled!");
        }

        private void GenerateRandomKey(Entry source, Entry target, int length)
        {
            alertLabel.IsVisible = false;
            if (length == 0)
            {
                length = Convert.FromBase64String(source.Text).Length;
            }
            var bytes = new byte[length];
            new Random().NextBytes(bytes);
            target.Text = Convert.ToBase64String(bytes);
        }

        private void ShowError(string errorMessage)
        {
            alertLabel.IsVisible = true;
            alertLabel.Text = errorMessage;
            alertLabel.TextColor = Colors.Red;
        }

        private bool IsBase64(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            Span<byte> buffer = new Span<byte>(new byte[input.Length]);
            return Convert.TryFromBase64String(input, buffer, out _);
        }

        static byte[] XOR(byte[] a, byte[] b)
        {
            var result = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = (byte)(a[i] ^ b[i]);
            }
            return result;
        }

        private void inputKeyA_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateKeyLengthLabel(inputKeyA.Text, labelA, "Key A");
        }

        private void inputKeyB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateKeyLengthLabel(inputKeyB.Text, labelB, "Key B");
        }

        private void UpdateKeyLengthLabel(string base64, Label label, string keyName)
        {
            if (string.IsNullOrWhiteSpace(base64))
            {
                label.Text = $"{keyName}:";
                return;
            }
            if (!IsBase64(base64))
            {
                label.Text = $"{keyName}: (parsing failed)";
                return;
            }

            byte[] bytes = Convert.FromBase64String(base64);
            int byteCount = bytes.Length;
            int bitCount = byteCount * 8;

            label.Text = $"{keyName}: ({byteCount} bytes / {bitCount} bits)";
        }

        private void showKeyA_Clicked(object sender, EventArgs e)
        {
            ShowHideKeys(showKeyA, inputKeyA);
        }

        private void showKeyB_Clicked(object sender, EventArgs e)
        {
            ShowHideKeys(showKeyB, inputKeyB);
        }

        private void showResult_Clicked(object sender, EventArgs e)
        {
            ShowHideKeys(showResult, resultEntry);
        }

        private void ShowHideKeys(Button button, Entry entry)
        {
            if(entry.IsPassword == true)
            {
                entry.IsPassword = false;
                button.Text = "Hide";
            }
            else
            {
                entry.IsPassword = true;
                button.Text = "Show";
            }
        }
    }
}