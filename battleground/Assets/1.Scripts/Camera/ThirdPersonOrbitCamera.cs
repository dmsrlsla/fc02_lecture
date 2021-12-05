using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�޶� �Ӽ��� �߿� �Ӽ� �ϳ��� ī�޶�κ��� ������ ����, �Ǻ� ������ ����
// ��ġ ������ ���ʹ� �浹ó�������� ����ϰ� �Ǻ� ������ ���ʹ� �ü��̵��� ����ϵ���
// �浹üũ : ���� �浹 üũ ���( ĳ���ͷκ��� ī�޶�, ī�޶�κ��� ĳ���� ����)
// ��ݹݵ��� ���� ���
// FOV ���� ���
[RequireComponent(typeof(Camera))]
public class ThirdPersonOrbitCamera : MonoBehaviour
{
    public Transform player; // �÷��̾� ��ġ
    public Vector3 pivotOffset = new Vector3(0.0f, 1.0f, 0.0f);
    public Vector3 camOffset = new Vector3(0.4f, 0.5f, -2.0f);

    public float smooth = 10f; // ī�޶� �����ӵ�

    public float horizontalAiminSpeed = 6.0f; // ���� ȸ���ӵ�
    public float verticalAiminSpeed = 6.0f; // ���� ȸ���ӵ�

    public float maxVerticalAngle = 30.0f; // �����ݵ��� �ִ�
    public float minVerticalAngle = -60.0f; // �����ݵ��� �ּ�

    public float recoilAngleBounds = 5.0f; // ��� �ݵ� �ٿ��.

    private float angleH = 0.0f; // ���콺 �̵��� ���� ī�޶� �����̵� ��ġ
    private float angleV = 0.0f; // ���콺 �̵��� ���� ī�޶� �����̵� ��ġ

    private Transform cameraTranform; // Ʈ������ ĳ��
    private Camera myCamera;
    private Vector3 relCameraPos; // �÷��̾�κ��� ī�޶� ������ ����.
    private float relCameraPosMag; // �÷��̾�κ��� ī�޶������ �Ÿ�.
    private Vector3 smoothPivotOffset; // ī�޶� �Ǻ� �������� ������ ����
    private Vector3 smoothCamOffset; // ī�޶� ��ġ�� ������ ����.
    private Vector3 targetPivotOffset; // ī�޶� �Ǻ� ������ ����
    private Vector3 targetCamOffset; // ī�޶� ��ġ�� ������ ����.

    private float defaultFOV; // �⺻ �þ߰�
    private float targetFOV; // Ÿ�� �þ߰�
    private float targetMaxVecticleAngle; // ī�޶� ���� �ִ� ����
    private float recoilAngle = 0f; // ��� �ݵ� ����.

    public float GetH
    {
        get { return angleH; }
    }

    public float GetV
    {
        get { return angleV; }
    }

    private void Awake()
    {
        // ĳ��
        cameraTranform = transform;
        myCamera = cameraTranform.GetComponent<Camera>();
        // ī�޶� �⺻ ������ ����
        cameraTranform.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
        cameraTranform.rotation = Quaternion.identity;

        relCameraPos = cameraTranform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;

        // �⺻����
        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;
        defaultFOV = myCamera.fieldOfView;
        angleH = player.eulerAngles.y;

        ResetTargetOffsets();
        ResetFOV();
        ResetMaxVerticalAngle();
    }

    public void ResetTargetOffsets()
    {
        targetPivotOffset = pivotOffset;
        targetCamOffset = camOffset;
    }

    public void ResetFOV()
    {
        this.targetFOV = defaultFOV;
    }

    public void ResetMaxVerticalAngle()
    {
        targetMaxVecticleAngle = this.maxVerticalAngle;
    }

    public void BoundVerical(float degree)
    {
        recoilAngle = degree;
    }

    public void SetTargetOffset(Vector3 newPivoitOffset, Vector3 newCamOffset)
    {
        targetPivotOffset = newPivoitOffset;
        targetCamOffset = newCamOffset;
    }

    public void SetFOV(float customFOV)
    {
        this.targetFOV = customFOV;
    }    

    bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
    {
        Vector3 target = player.position + (Vector3.up * deltaPlayerHeight);
        if(Physics.SphereCast(checkPos, 0.2f, target - checkPos, out RaycastHit hit, relCameraPosMag))
        {
            if(hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }

    bool ReversePosCheck(Vector3 checkPos, float deltaPlayerHeight, float maxDistance)
    {
        Vector3 origin = player.position + (Vector3.up * deltaPlayerHeight);
        if (Physics.SphereCast(checkPos, 0.2f, checkPos - origin, out RaycastHit hit, maxDistance))
        {
            if (hit.transform != player && !hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }

    bool DoubleViewingPosCheck(Vector3 checkPos, float offset)
    {
        float PlayerFocusHeight = player.GetComponent<CapsuleCollider>().height * 0.75f;

        return ViewingPosCheck(checkPos, PlayerFocusHeight) && ReversePosCheck(checkPos, PlayerFocusHeight, offset);
    }

    private void Update()
    {
        // ���콺 �̵���
        angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), - 1f, 1f) * horizontalAiminSpeed;
        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1f, 1f) * verticalAiminSpeed;

        // ���� �̵� ����
        angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVecticleAngle);
        // ���� ī�޶� �ٿ
        angleV = Mathf.LerpAngle(angleV, angleV + recoilAngle, 10 * Time.deltaTime);

        // ī�޶� ����
        Quaternion camYRotation = Quaternion.Euler(0.0f, angleH, 0.0f);
        Quaternion aimRotation = Quaternion.Euler(- angleV, angleH, 0.0f);

        cameraTranform.rotation = aimRotation;

        // Set Fov
        myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, targetFOV, Time.deltaTime);

        Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
        Vector3 noCollisionOffset = targetCamOffset; //  �����Ҷ� ī�޶� �����°�, �����Ҷ� ��ҿ� �ٸ�.

        for(float zoffset = targetCamOffset.z; zoffset <=0f; zoffset+=0.5f)
        {
            noCollisionOffset.z = zoffset;

            if(DoubleViewingPosCheck(baseTempPosition+aimRotation * noCollisionOffset,
                Mathf.Abs(zoffset)) || zoffset == 0f)
            {
                break;
            }
        }

        // ����¡ ī�޶�
        smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
        smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, smooth * Time.deltaTime);

        // ī�޶� ������ ����
        cameraTranform.position = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;

        if(recoilAngle > 0.0f)
        {
            recoilAngle -= recoilAngleBounds * Time.deltaTime;
        }
        else if(recoilAngle < 0.0f)
        {
            recoilAngle += recoilAngleBounds * Time.deltaTime;
        }
    }

    public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
    {
        return Mathf.Abs((finalPivotOffset - smoothPivotOffset).magnitude);
    }
}
