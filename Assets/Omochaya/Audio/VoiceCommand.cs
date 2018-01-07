// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VoiceCommand.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The voice command.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Audio
{
    using System;
    using System.Collections.Generic;
    using Omochaya.Common;
    using Omochaya.Debug;
    using UnityEngine;

    /// <summary>The voice command.</summary>
    public class VoiceCommand : MonoBehaviour
    {
        /// <summary>The model.</summary>
        private class Model
        {
            /// <summary>The data.</summary>
            private struct Data
            {
                /// <summary>The level.</summary>
                public int Level { get; private set; }

                /// <summary>The volume.</summary>
                public float Volume { get; private set; }

                /// <summary>The constructer.</summary>
                public Data(int level, float volume) { Level = level; Volume = volume; }

                /// <summary>The compare.</summary>
                public static int Compare(Data a, Data b) { return a.Volume < b.Volume ? -1 : +1; }
            }

            /// <summary>The min.</summary>
            private const float Min = 0.01f;

            /// <summary>The unit.</summary>
            private const float Unit = 0.01f;

            /// <summary>The tag.</summary>
            public string Tag { get; private set; }

            /// <summary>The bias.</summary>
            public float Bias { get; private set; }

            /// <summary>The data.</summary>
            private Data[] data;

            /// <summary>The constructer.</summary>
            public Model(string tag, float bias, float[] samples)
            {
                this.Tag = tag;
                this.Bias = bias;
                var list = new List<Data>();
                for (var i=0; i<samples.Length; i++)
                {
                    if(Min < samples[i])
                    {
                        list.Add(new Data(i, samples[i]));
                    }
                }
                list.Sort(Data.Compare);
                this.data = list.ToArray();
            }

            /// <summary>The get score.</summary>
            public float GetScore(float[] samples, float cand)
            {
                var ret = 100f;
                foreach (var data in this.data)
                {
                    var d = (samples[data.Level] - data.Volume) / Unit;
                    if (d < 0)
                    {
                        d = -d;
                    }
                    ret -= d * d / this.Bias;
                    if (ret < cand)
                    {
                        return 0;
                    }
                }
                return ret;
            }
        }

        /// <summary>The frequency.</summary>
        private const int Frequency = 44100;

        /// <summary>The sample count.</summary>
        private const int SampleCount = 2048;

        /// <summary>The check count.</summary>
        private const int CheckCount = 128;

        /// <summary>The mini volume.</summary>
        private const float MiniVolume = 0.007f;

        /// <summary>The model data.</summary>
        private List<Model> models = new List<Model>() {
            new Model("あ", 1f, new float[]{
0.00384849f, 0.00493159f, 0.01047390f, 0.02546350f, 0.03641597f, 0.02516375f, 0.01159107f, 0.00769371f,
0.01527912f, 0.01808841f, 0.00987192f, 0.00812476f, 0.00839107f, 0.00692805f, 0.00476481f, 0.00438806f,
0.00391315f, 0.00644382f, 0.00599509f, 0.00411229f, 0.00461220f, 0.00638015f, 0.01539177f, 0.01234319f,
0.00246969f, 0.00589252f, 0.01570145f, 0.02561386f, 0.02158331f, 0.01276624f, 0.01475436f, 0.04797980f,
0.04273915f, 0.00944350f, 0.01233287f, 0.02790152f, 0.03904518f, 0.02229502f, 0.00618176f, 0.01227358f,
0.04043438f, 0.04194562f, 0.02384187f, 0.01930255f, 0.02614001f, 0.04817113f, 0.04216288f, 0.02458868f,
0.02104592f, 0.02997278f, 0.02560730f, 0.00659453f, 0.00346623f, 0.00278298f, 0.00284550f, 0.00299069f,
0.00248883f, 0.00239817f, 0.00211437f, 0.00155641f, 0.00165671f, 0.00213022f, 0.00181430f, 0.00151036f,
0.00150863f, 0.00111498f, 0.00141160f, 0.00147482f, 0.00118803f, 0.00135725f, 0.00133473f, 0.00123278f,
0.00095485f, 0.00105125f, 0.00116358f, 0.00102895f, 0.00075083f, 0.00077359f, 0.00087332f, 0.00082607f,
0.00096800f, 0.00111112f, 0.00086252f, 0.00065894f, 0.00104988f, 0.00111133f, 0.00093256f, 0.00065463f,
0.00037060f, 0.00054083f, 0.00042274f, 0.00015479f, 0.00037688f, 0.00064354f, 0.00062400f, 0.00047110f,
0.00041281f, 0.00041038f, 0.00036350f, 0.00023170f, 0.00052488f, 0.00066331f, 0.00065154f, 0.00083998f,
0.00058349f, 0.00055056f, 0.00039827f, 0.00009744f, 0.00018864f, 0.00035755f, 0.00042552f, 0.00044462f,
0.00047665f, 0.00032387f, 0.00035416f, 0.00057230f, 0.00053907f, 0.00032465f, 0.00039603f, 0.00038288f,
0.00029867f, 0.00038324f, 0.00035129f, 0.00046357f, 0.00042298f, 0.00021853f, 0.00033269f, 0.00087060f,
            }),
            new Model("い", 1f, new float[]{
0.01113202f, 0.01591617f, 0.01777293f, 0.02641688f, 0.05520972f, 0.04858197f, 0.02052515f, 0.01736703f,
0.10195640f, 0.09926480f, 0.01274795f, 0.01638292f, 0.04430021f, 0.08352802f, 0.06426221f, 0.02515547f,
0.01792350f, 0.01447989f, 0.01094844f, 0.00831717f, 0.00962525f, 0.00964335f, 0.00788540f, 0.00796515f,
0.00714714f, 0.00573860f, 0.00626525f, 0.00606379f, 0.00496609f, 0.00478785f, 0.01006241f, 0.01197431f,
0.00666197f, 0.00358702f, 0.00262427f, 0.00333516f, 0.00404007f, 0.00484781f, 0.00748403f, 0.00642551f,
0.00338614f, 0.00237963f, 0.00139766f, 0.00168660f, 0.00164684f, 0.00391364f, 0.00492037f, 0.00285346f,
0.00336999f, 0.00361549f, 0.00405777f, 0.00390507f, 0.00331955f, 0.00340750f, 0.00226209f, 0.00236623f,
0.00216363f, 0.00266641f, 0.00328720f, 0.00238637f, 0.00153693f, 0.00142973f, 0.00241513f, 0.00163745f,
0.00177219f, 0.00162495f, 0.00267076f, 0.00388746f, 0.00260840f, 0.00197208f, 0.00150766f, 0.00182587f,
0.00155720f, 0.00123913f, 0.00129577f, 0.00089379f, 0.00102912f, 0.00144073f, 0.00163826f, 0.00164473f,
0.00185641f, 0.00120758f, 0.00092815f, 0.00175746f, 0.00193835f, 0.00221593f, 0.00236961f, 0.00128092f,
0.00091667f, 0.00097089f, 0.00070900f, 0.00187951f, 0.00156696f, 0.00060268f, 0.00104471f, 0.00148951f,
0.00111219f, 0.00052882f, 0.00065689f, 0.00102641f, 0.00107573f, 0.00106390f, 0.00115577f, 0.00102876f,
0.00113058f, 0.00178821f, 0.00181100f, 0.00174408f, 0.00221601f, 0.00130884f, 0.00048750f, 0.00087961f,
0.00126502f, 0.00127554f, 0.00135107f, 0.00140176f, 0.00083406f, 0.00052672f, 0.00123757f, 0.00118216f,
0.00094244f, 0.00137557f, 0.00134674f, 0.00123651f, 0.00123114f, 0.00128920f, 0.00191518f, 0.00190249f,
            }),
            new Model("う", 1f, new float[]{
0.00791920f, 0.01047153f, 0.01188092f, 0.02058822f, 0.04484797f, 0.04143989f, 0.01856514f, 0.01484269f,
0.04320443f, 0.07363541f, 0.06043128f, 0.05108990f, 0.04311535f, 0.08012344f, 0.08995502f, 0.04702154f,
0.04935139f, 0.03583625f, 0.03028063f, 0.03196486f, 0.01694932f, 0.00677957f, 0.00337654f, 0.00592123f,
0.00565608f, 0.00469282f, 0.00392013f, 0.00243694f, 0.00275944f, 0.00304477f, 0.00235199f, 0.00183899f,
0.00200095f, 0.00234429f, 0.00231281f, 0.00310234f, 0.00269253f, 0.00199253f, 0.00269175f, 0.00241388f,
0.00231221f, 0.00267157f, 0.00169192f, 0.00105676f, 0.00168903f, 0.00155076f, 0.00138464f, 0.00157541f,
0.00113301f, 0.00073474f, 0.00063349f, 0.00090178f, 0.00203240f, 0.00375617f, 0.00339872f, 0.00198997f,
0.00423999f, 0.00815277f, 0.00675918f, 0.00228834f, 0.00156097f, 0.00228294f, 0.00197807f, 0.00189816f,
0.00167370f, 0.00176669f, 0.00216478f, 0.00142900f, 0.00104091f, 0.00165536f, 0.00171883f, 0.00127389f,
0.00091291f, 0.00092454f, 0.00114125f, 0.00097268f, 0.00083316f, 0.00122057f, 0.00146494f, 0.00104163f,
0.00114821f, 0.00128659f, 0.00121805f, 0.00118660f, 0.00111040f, 0.00104144f, 0.00073064f, 0.00052709f,
0.00061836f, 0.00097439f, 0.00072122f, 0.00043205f, 0.00047200f, 0.00068141f, 0.00128224f, 0.00098234f,
0.00044523f, 0.00056591f, 0.00094338f, 0.00084864f, 0.00046043f, 0.00047415f, 0.00059620f, 0.00093957f,
0.00079724f, 0.00092797f, 0.00087141f, 0.00063328f, 0.00097927f, 0.00071673f, 0.00055137f, 0.00054593f,
0.00065512f, 0.00090458f, 0.00054669f, 0.00040021f, 0.00081707f, 0.00074827f, 0.00062711f, 0.00069032f,
0.00040153f, 0.00030081f, 0.00053127f, 0.00077683f, 0.00073337f, 0.00055381f, 0.00037215f, 0.00047923f,
            }),
            new Model("え", 1f, new float[]{
0.00154518f, 0.00218910f, 0.00335719f, 0.01322311f, 0.03988073f, 0.03102464f, 0.00609672f, 0.00755855f,
0.01263051f, 0.03293251f, 0.02783501f, 0.00972249f, 0.01326967f, 0.01968180f, 0.03978189f, 0.03110949f,
0.00880420f, 0.00857996f, 0.01998087f, 0.05974036f, 0.04629041f, 0.00757581f, 0.00696624f, 0.01103382f,
0.03084797f, 0.02689069f, 0.00682586f, 0.00469434f, 0.00736294f, 0.01069480f, 0.00680617f, 0.00240276f,
0.00366753f, 0.00736160f, 0.00864343f, 0.00485909f, 0.00382626f, 0.00499263f, 0.01384501f, 0.01826602f,
0.00941788f, 0.00312841f, 0.00223286f, 0.00916201f, 0.01467891f, 0.00988644f, 0.00434994f, 0.00284119f,
0.00395988f, 0.00444594f, 0.00377844f, 0.00371579f, 0.00337778f, 0.00408609f, 0.00316607f, 0.00149015f,
0.00150842f, 0.00248846f, 0.00363584f, 0.00316986f, 0.00228997f, 0.00207820f, 0.00168939f, 0.00184298f,
0.00174882f, 0.00180575f, 0.00168588f, 0.00129038f, 0.00151831f, 0.00152494f, 0.00168444f, 0.00181902f,
0.00229807f, 0.00547186f, 0.00641593f, 0.00482915f, 0.00335618f, 0.00411200f, 0.00967714f, 0.00759504f,
0.00236245f, 0.00153852f, 0.00147603f, 0.00272786f, 0.00277892f, 0.00199798f, 0.00227022f, 0.00664204f,
0.01339213f, 0.01110328f, 0.00623505f, 0.00646720f, 0.00889832f, 0.01104762f, 0.00598953f, 0.00367585f,
0.00384695f, 0.00387675f, 0.00471723f, 0.00245755f, 0.00140913f, 0.00128698f, 0.00128684f, 0.00117484f,
0.00138772f, 0.00238268f, 0.00240659f, 0.00334612f, 0.00472948f, 0.00503994f, 0.00515335f, 0.00684837f,
0.01030571f, 0.01035406f, 0.00714312f, 0.00322525f, 0.00404548f, 0.00771502f, 0.00787014f, 0.00571213f,
0.00300412f, 0.00338533f, 0.00506929f, 0.00410161f, 0.00223032f, 0.00139222f, 0.00107154f, 0.00137023f,
            }),
            new Model("お", 1f, new float[]{
0.00035203f, 0.00082284f, 0.00097878f, 0.00422196f, 0.00707248f, 0.00947727f, 0.01570257f, 0.03487365f,
0.03727481f, 0.01841075f, 0.00686481f, 0.01860347f, 0.02953266f, 0.01575210f, 0.00701801f, 0.01036218f,
0.01151183f, 0.01742232f, 0.01520999f, 0.02860191f, 0.04952534f, 0.03594397f, 0.03819717f, 0.03346219f,
0.02261158f, 0.02199508f, 0.01357528f, 0.01126474f, 0.00679287f, 0.00493770f, 0.01412903f, 0.01713319f,
0.01860296f, 0.02921269f, 0.02718815f, 0.02792769f, 0.03871287f, 0.05562536f, 0.07450452f, 0.04193536f,
0.01033799f, 0.01222418f, 0.00910391f, 0.00889894f, 0.00735960f, 0.00599697f, 0.00468089f, 0.00339312f,
0.00331647f, 0.00298980f, 0.00267722f, 0.00239879f, 0.00167919f, 0.00162927f, 0.00200574f, 0.00153145f,
0.00111077f, 0.00086620f, 0.00080560f, 0.00113878f, 0.00128991f, 0.00122990f, 0.00106209f, 0.00107703f,
0.00121200f, 0.00092580f, 0.00086282f, 0.00101333f, 0.00116417f, 0.00116707f, 0.00095024f, 0.00075366f,
0.00071396f, 0.00059817f, 0.00032952f, 0.00086884f, 0.00125746f, 0.00080626f, 0.00071677f, 0.00093098f,
0.00068615f, 0.00070055f, 0.00104547f, 0.00082621f, 0.00069836f, 0.00077946f, 0.00081971f, 0.00069110f,
0.00047048f, 0.00023121f, 0.00039521f, 0.00096881f, 0.00097888f, 0.00063316f, 0.00071718f, 0.00056254f,
0.00031232f, 0.00070549f, 0.00073736f, 0.00036369f, 0.00024533f, 0.00015389f, 0.00022081f, 0.00034019f,
0.00040907f, 0.00057355f, 0.00060914f, 0.00050869f, 0.00049389f, 0.00040029f, 0.00019105f, 0.00012499f,
0.00019559f, 0.00022750f, 0.00021393f, 0.00043879f, 0.00065199f, 0.00073055f, 0.00056973f, 0.00022724f,
0.00029039f, 0.00063468f, 0.00045049f, 0.00022081f, 0.00026465f, 0.00033152f, 0.00034486f, 0.00019406f,
            }),
        };

        /// <summary>The audio clip.</summary>
        private AudioClip audioClip = null;

        /// <summary>The scenario.</summary>
        private Scenario scenario = null;

        /// <summary>The is start.</summary>
        private bool isStart = false;

        /// <summary>The seek.</summary>
        private int seek = 0;

        /// <summary>The fft buf.</summary>
        private float[] fftBuf;

        /// <summary>The fft ip.</summary>
        private int[] fftIp;

        /// <summary>The fft work.</summary>
        private float[] fftWork;

        /// <summary>The check data.</summary>
        private float[] checkData;

        /// <summary>The volume.</summary>
        public float Volume { get; private set; }

        /// <summary>The moji.</summary>
        public string Moji { get; private set; }

        /// <summary>The check length.</summary>
        public float CheckLenght { get { return this.fftBuf != null && this.audioClip ? this.fftBuf.Length / 2 / this.audioClip.frequency : 0; } }

        /// <summary>The is start.</summary>
        public bool IsStart() { return this.isStart; }

        /// <summary>The start.</summary>
        private void Start()
        {
            Prepare(Frequency, SampleCount, CheckCount);
        }

        /// <summary>The prepare.</summary>
        private void Prepare(int frequency, int sampleCount, int checkCount)
        {
            this.isStart = false;
            this.Moji = string.Empty;
            this.seek = 0;
            var audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.clip = Microphone.Start(null, true, 1, frequency);
            this.audioClip = audioSource.clip;
            this.fftBuf = new float[sampleCount * 2];
            this.fftIp = new int[2 + (int)Math.Sqrt(sampleCount) + 1];
            this.fftWork = new float[sampleCount / 2];
            this.checkData = new float[checkCount];
            this.scenario = new Scenario(Scenario());
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            this.scenario.Update();
        }

        /// <summary>The scenario.</summary>
        private IEnumerator<Func<bool>> Scenario()
        {
            // マイクの準備完了を待つ
            while (Microphone.GetPosition(null) <= 0)
            {
                yield return null;
            }

            this.isStart = true;
            var moji = string.Empty;
            var time = 0f;
            while (true)
            {
                this.UpdateSeek();
                this.UpdateSpectrum();
                this.UpdateCheckData();
                var cand = this.selectMoji();
                time += Time.deltaTime;
                if (moji != cand || true)//0.2f < time)
                {
                    time = 0f;
                    this.Moji = moji = cand;
                    if (!string.IsNullOrEmpty(moji))
                    {
                        DebugLog.Put(moji);
                    }
                }
                else
                {
                    this.Moji = string.Empty;
                }
                yield return null;
            }
        }

        /// <summary>The select moji.</summary>
        private string selectMoji()
        {
            if (this.Volume < MiniVolume)
            {
                return string.Empty;
            }
            var ret = string.Empty;
#if true
            var best = 0f;
            var log = string.Empty;
            foreach (var model in this.models)
            {
                var score = model.GetScore(this.checkData, 0);
                log += "[" + model.Tag + "]" + score.ToString("G3") + ", ";
                if (best < score)
                {
                    best = score;
                    ret = model.Tag;
                }
            }
            DebugLog.Put(log);
#else
            var log = string.Empty;
            var j = 0;
            foreach (var f in this.checkData)
            {
                log += f.ToString("F8") + "f, ";
                if (++j % 8 == 0)
                {
                    log += "\n";
                }
            }
            DebugLog.Put(log);
            ret = "あ";
#endif
            return ret;
        }

        /// <summary>The get spectrum data.</summary>
        private void UpdateSeek()
        {
            var seeku = (int)(this.audioClip.frequency * Time.deltaTime);
            var seekr = Microphone.GetPosition(null);
            var seekv = this.seek + seeku;
            var seekd = seekr - seekv;
            // ループしてるのでループ前で計算
            if (seekd < -this.audioClip.frequency / 2)
            {
                seekr += this.audioClip.frequency;
                seekd = seekr - seekv;
            }
            else if (this.audioClip.frequency / 2 < seekd)
            {
                seekv += this.audioClip.frequency;
                seekd = seekr - seekv;
            }
            // 予想より遅れていたらそちらが正しい。というか合わせるしかない。
            if (seekd < 0)
            {
                seekv = seekr;

            }
            // 予想が遅れすぎないようにする
            else if (seekd < seeku * 2)
            {
                seekv = seekr - seeku * 2;
            }
            // 何れにせよ結果的な予想が正しいとする
            seekv %= this.audioClip.frequency;
            while (seekv < 0)
            {
                seekv += this.audioClip.frequency;
            }
            this.seek = seekv;
        }

        /// <summary>The update spectrum.</summary>
        private void UpdateSpectrum()
        {
            var maxSize = (int)(this.audioClip.frequency * this.audioClip.length);
            var smpSize = this.fftBuf.Length / 2;
            var end = this.seek;
            var start = end - smpSize;
            var volume = 0f;
            var index = 0;
            if (start < 0)
            {
                // バッファの後端に余ってるぶん
                var src = new float[-start];
                this.audioClip.GetData(src, maxSize + start);
                foreach (var f in src)
                {
                    this.fftBuf[index++] = f;
                    this.fftBuf[index++] = 0;
                    volume += 0 < f ? f : -f;
                }
                // バッファの先頭から終端まで
                if (0 < end)
                {
                    src = new float[end];
                    this.audioClip.GetData(src, 0);
                    foreach (var f in src)
                    {
                        this.fftBuf[index++] = f;
                        this.fftBuf[index++] = 0;
                        volume += 0 < f ? f : -f;
                    }
                }
            }
            else
            {
                // 範囲が取れた
                var src = new float[smpSize];
                this.audioClip.GetData(src, start);
                foreach (var f in src)
                {
                    this.fftBuf[index++] = f;
                    this.fftBuf[index++] = 0;
                    volume += 0 < f ? f : -f;
                }
            }

            // 音量
            this.Volume = volume / smpSize;

            // 音量が低い場合は判定しない
            if (this.Volume < MiniVolume)
            {
                this.fftBuf.Initialize();
            }
            else
            {
                this.fftIp[0] = 0;
                Fft4g.cdft(smpSize * 2, -1, this.fftBuf, this.fftIp, this.fftWork);
            }
        }

        /// <summary>The update check data.</summary>
        private void UpdateCheckData()
        {
            this.checkData.Initialize();
            if(this.Volume < MiniVolume)
            {
                return;
            }

            // fftBuf は左右同じなので真ん中で反転させて合計する。あと正負は関係ないので正に統一。
            var num = this.checkData.Length * 2;// this.fftBuf.Length / 2;
            var unit = (num + this.checkData.Length - 1) / this.checkData.Length;
            for (var i = 0; i < num; i++)
            {
                var f = this.fftBuf[i];
                this.checkData[i / unit] += 0 < f ? f : -f;
                f = this.fftBuf[this.fftBuf.Length - i - 1];
                this.checkData[i / unit] += 0 < f ? f : -f;
            }

            // 正規化
            var sum = 0f;
            for (var i = 0; i < this.checkData.Length; i++)
            {
                sum += this.checkData[i];
            }
            if(0 < sum)
            {
                for (var i = 0; i < this.checkData.Length; i++)
                {
                    this.checkData[i] /= sum;
                }
            }
            else
            {
                for (var i = 0; i < this.checkData.Length; i++)
                {
                    this.checkData[i] = 0;
                }
            }
        }
    }
}