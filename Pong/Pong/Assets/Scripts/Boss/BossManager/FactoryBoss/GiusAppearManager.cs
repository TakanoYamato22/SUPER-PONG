using System.Collections;
using UnityEngine;

public class GiusAppearManager : MonoBehaviour
{
    [Header("�{�X�ݒ�")]
    [SerializeField] private Transform boss;
    [SerializeField] private BossController bossController;
    [SerializeField] private GiusController giusController;


    [Header("�o��ʒu")]
    [SerializeField] private Vector3 startPosition;

    [Tooltip("�o���Ɏ~�܂�ʒu�BY��2�ɂ������ꍇ��Y��2�ɐݒ�")]
    [SerializeField]
    private Vector3 targetPosition =
        new Vector3(0f, 2f, 0f);

    [Header("�o��ړ�")]
    [Tooltip("�J�n�ʒu����o��ʒu�܂ňړ����鎞��")]
    [SerializeField] private float appearTime = 1.5f;

    [Header("�o�ꊮ��Effect")]
    [Tooltip("Hierarchy��ɒu�����o��Effect�̃��[�g")]
    [SerializeField] private GameObject appearEffectRoot;

    [Tooltip("Effect�Đ���A�ړ��J�n�܂ő҂���")]
    [SerializeField] private float effectWaitTime = 0.8f;

    [Header("���ʉ�")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip appearSE;

    private ParticleSystem[] appearParticles;

    private Coroutine appearCoroutine;

    private float savedTimeScale = 1f;
    private bool appearing;

    private void Awake()
    {
        InitializeAppearEffect();
    }

    private void Start()
    {
        appearCoroutine =
            StartCoroutine(AppearCoroutine());
    }

    private IEnumerator AppearCoroutine()
    {
        if (boss == null)
        {
            Debug.LogWarning(
                "GiusAppearManager�FBoss���ݒ肳��Ă��܂���I",
                this
            );

            yield break;
        }

        appearing = true;

        savedTimeScale = Time.timeScale;

        // Gius�ȊO�̎��Ԃ��~
        Time.timeScale = 0f;

        // �o�ꒆ�͐퓬�������~
        if (bossController != null)
        {
            bossController.SetBattleStarted(false);
        }

        boss.position = startPosition;

        if (audioSource != null &&
            appearSE != null)
        {
            audioSource.PlayOneShot(appearSE);
        }

        float timer = 0f;
        float safeAppearTime =
            Mathf.Max(0.01f, appearTime);

        // �J�n�ʒu����targetPosition�܂œo��
        while (timer < safeAppearTime)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(
                timer / safeAppearTime
            );

            float smoothT =
                Mathf.SmoothStep(0f, 1f, t);

            boss.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                smoothT
            );

            yield return null;
        }

        // �K���ړI�n�ɍ��킹��
        boss.position = targetPosition;

        Debug.Log(
            $"Gius�F�o��ʒu�ɓ��� Y={boss.position.y}",
            this
        );

        // Y=2�t�߂ɓ�����������Effect�Đ�
        PlayAppearEffect();

        // TimeScale��0�ł��҂Ă�
        yield return new WaitForSecondsRealtime(
            effectWaitTime
        );

        StopAppearEffect();

        // Effect�I����ɃQ�[���S�̂��ĊJ
        RestoreTimeScale();

        appearing = false;

        // Effect���I����Ă���ړ��J�n
        if (giusController != null)
        {
            giusController.StartMove();
        }

        // Effect���I����Ă���퓬�J�n
        if (bossController != null)
        {
            bossController.SetBattleStarted(true);
        }

        appearCoroutine = null;

        Debug.Log(
            "Gius�F�o��Effect�I���A�ړ��E�퓬�J�n",
            this
        );
    }

    private void InitializeAppearEffect()
    {
        if (appearEffectRoot == null)
        {
            Debug.LogWarning(
                "Appear Effect Root���ݒ肳��Ă��܂���B",
                this
            );

            return;
        }

        appearEffectRoot.SetActive(true);

        appearParticles =
            appearEffectRoot.GetComponentsInChildren<ParticleSystem>(
                true
            );

        if (appearParticles == null ||
            appearParticles.Length == 0)
        {
            Debug.LogWarning(
                "Appear Effect Root����Particle System������܂���B",
                appearEffectRoot
            );

            return;
        }

        foreach (ParticleSystem particle in appearParticles)
        {
            if (particle == null)
            {
                continue;
            }

            ParticleSystem.MainModule main =
                particle.main;

            // Time.timeScale = 0�ł�������
            main.useUnscaledTime = true;

            particle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        Debug.Log(
            $"Gius�o��Effect�������FParticle�� {appearParticles.Length}",
            appearEffectRoot
        );
    }

    private void PlayAppearEffect()
    {
        if (appearEffectRoot == null)
        {
            Debug.LogWarning(
                "Appear Effect Root���ݒ肳��Ă��܂���B",
                this
            );

            return;
        }

        appearEffectRoot.SetActive(true);

        // Effect���{�X�̓o��ʒu�֍��킹��
        appearEffectRoot.transform.position =
            boss.position;

        if (appearParticles == null ||
            appearParticles.Length == 0)
        {
            appearParticles =
                appearEffectRoot.GetComponentsInChildren<ParticleSystem>(
                    true
                );
        }

        if (appearParticles == null ||
            appearParticles.Length == 0)
        {
            Debug.LogWarning(
                "�Đ��ł���Particle System������܂���B",
                appearEffectRoot
            );

            return;
        }

        foreach (ParticleSystem particle in appearParticles)
        {
            if (particle == null)
            {
                continue;
            }

            particle.gameObject.SetActive(true);

            ParticleSystem.MainModule main =
                particle.main;

            main.useUnscaledTime = true;

            particle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );

            particle.Clear(true);
            particle.Play(true);
        }

        Debug.Log(
            $"Gius�o��Effect�Đ��FParticle�� {appearParticles.Length}",
            appearEffectRoot
        );
    }

    private void StopAppearEffect()
    {
        if (appearParticles == null)
        {
            return;
        }

        foreach (ParticleSystem particle in appearParticles)
        {
            if (particle == null)
            {
                continue;
            }

            particle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }
    }

    private void RestoreTimeScale()
    {
        Time.timeScale = savedTimeScale;
    }

    private void OnDisable()
    {
        if (appearCoroutine != null)
        {
            StopCoroutine(appearCoroutine);
            appearCoroutine = null;
        }

        StopAppearEffect();

        if (appearing)
        {
            RestoreTimeScale();
        }

        appearing = false;
    }
}