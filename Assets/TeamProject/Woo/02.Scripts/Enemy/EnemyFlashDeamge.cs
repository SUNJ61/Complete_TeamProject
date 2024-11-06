using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlashDamage : MonoBehaviour
{
    public int Demon_Counter = 0;
    float timer;
    bool isSoundPlay;
    [SerializeField] bool isFlashing;
    NavMeshAgent Enemyagent;
    Animator Enemyanimator;
    [SerializeField] Enemy enemy;
    [SerializeField] ParticleSystem particle_somoke;
    [SerializeField] CapsuleCollider Demon_cap;
    [SerializeField] AudioClip Demon_Steam;

    [SerializeField] private Collider currentCollider; // ���� �浹ü ����

    private void Start()
    {
        Demon_Steam = Resources.Load<AudioClip>("Sound/Demon/Demon_Steam");

        Demon_cap = GetComponent<CapsuleCollider>();
        timer = 0f;
        isFlashing = false;
        Enemyagent = GetComponent<NavMeshAgent>();
        Enemyanimator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        particle_somoke = transform.GetChild(5).GetComponent<ParticleSystem>();
        particle_somoke.Stop();
    }

    private void OnEnable()
    {
        if (Enemyagent == null)
        {
            return;
        }
        else
        {
            timer = 0;
            Enemyagent.isStopped = false;
            Enemyagent.speed = 5;
            Demon_cap.enabled = true;
            particle_somoke.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enemy.DemonDie)
        {
            if (other.gameObject.CompareTag("FlashCol"))
            {
                currentCollider = other; // ���� �浹ü ����
                Debug.Log(currentCollider);

                if (currentCollider.enabled) // �ݶ��̴��� Ȱ��ȭ�Ǿ� ���� ��
                {

                    isFlashing = true;
                    particle_somoke.Play();

                    // ���� ��� ����
                    if (!isSoundPlay && Demon_Steam != null)
                    {
                        Demon_Steam.name = $"Demon_Steam_{Demon_Counter}";
                        InGameSoundManager.instance.ActiveSound(gameObject, Demon_Steam, 10, true, true, true, 1);
                        isSoundPlay = true;
                    }


                    StartCoroutine(FlashingCoroutine()); // �ڷ�ƾ ����
                }
            }

        }
        
    }

    public IEnumerator FlashingCoroutine()
    {
        while (isFlashing && timer < 3f)
        {
            print(timer);
            if (currentCollider != null && currentCollider.enabled) // �ݶ��̴��� Ȱ��ȭ�� ���
            {
                timer += Time.deltaTime; // Ÿ�̸� ����

                if (timer >= 2f)
                {
                    if (isSoundPlay)
                    {
                        InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
                        InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
                        isSoundPlay = false;
                        timer = 0f;
                        isFlashing = false;
                        if (enemy.Killplayer == false)
                        {
                            Demon_cap.enabled = false; // �浹 ��Ȱ��ȭ
                            particle_somoke.Stop();
                            Enemyagent.isStopped = true; // �̵� ����
                            Enemyagent.speed = 0;
                            Enemyanimator.SetTrigger("Flash"); // �ִϸ��̼� Ʈ����

                            yield return new WaitForSeconds(4.5f); // ���
                            if(enemy.DemonDie == false)
                            {
                                Demon_cap.enabled = true; // �浹 Ȱ��ȭ
                                Enemyagent.speed = 5;
                                Enemyagent.isStopped = false; // �ٽ� �̵� ����
                            }
                        }
                    }
                }

            }
            else
            {
                HandleExitState(); // �ݶ��̴��� ��Ȱ��ȭ�� ��� ���� ����
                yield break; // �ڷ�ƾ ����
            }

            yield return null; // ���� �����ӱ��� ���
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == currentCollider)
        {
            HandleExitState(); // ���� ���� ó��
            currentCollider = null; // ���� �浹ü �ʱ�ȭ
        }
    }

    public void HandleExitState()
    {
     
        isFlashing = false; // �÷��� ���� ����

        if (isSoundPlay && InGameSoundManager.instance.Data.ContainsKey($"Demon_Steam_{Demon_Counter}"))
        {
            InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
            InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
        }

        // ���� �ʱ�ȭ
        timer = 0f; // Ÿ�̸� �ʱ�ȭ
        particle_somoke.Stop(); // ��ƼŬ ����
        isSoundPlay = false; // ���� ���� �ʱ�ȭ
    }
}
