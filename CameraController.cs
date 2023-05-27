//Owner : NamNguyen
//Email: nguyennam4work@gmail.com

using UnityEngine;
public class CameraController : MonoBehaviour
{
    public Transform target; // Đối tượng để xoay quanh
    public float rotateSpeed = 5f; // Tốc độ xoay
    public float zoomSpeed = 2000f; // Tốc độ phóng to thu nhỏ
    public float panSpeed = 100f; // Tốc độ pan
    public float maxPanDelta = 100f; // Giá trị tối đa của delta di chuyển chuột

    private Vector3 offset; // Khoảng cách giữa camera và target
    private Vector3 lastMousePosition; // Vị trí chuột trước đó
    private bool isPanning; // Trạng thái pan của camera

    private void Start()
    {
        offset = transform.position - target.position;
        lastMousePosition = Input.mousePosition;
    }

    private void LateUpdate()
    {
        // Xoay quanh target
        if (!isPanning && Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;

            // Xoay camera quanh target
            Quaternion horizontalRotation = Quaternion.Euler(0f, mouseX, 0f);
            Quaternion verticalRotation = Quaternion.Euler(-mouseY, 0f, 0f);
            offset = horizontalRotation * verticalRotation * offset;
        }

        // Phóng to thu nhỏ
        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        // Cập nhật khoảng cách từ camera đến target
        float distance = offset.magnitude;
        float newDistance = distance - zoom;

        // Giới hạn khoảng cách tối thiểu
        float minDistance = 2f;
        if (newDistance < minDistance)
        {
            newDistance = minDistance;
        }

        // Cập nhật vị trí camera
        offset = offset.normalized * newDistance;
        transform.position = target.position + offset;

        // Kiểm tra sự kiện pan
        if (!isPanning && Input.GetMouseButton(0))
        {
            isPanning = true;
        }

        if (isPanning)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

            // Giới hạn delta di chuyển chuột
            mouseDelta.x = Mathf.Clamp(mouseDelta.x, -maxPanDelta, maxPanDelta);
            mouseDelta.y = Mathf.Clamp(mouseDelta.y, -maxPanDelta, maxPanDelta);

            Vector3 pan = -transform.right * mouseDelta.x * panSpeed * Time.deltaTime
                + -transform.forward * mouseDelta.y * panSpeed * Time.deltaTime;
            pan.y = 0;
            target.position += pan;

            if (Input.GetMouseButtonUp(0))
            {
                isPanning = false;
            }
        }

        lastMousePosition = Input.mousePosition;
        // Giữ camera nhìn về target
        if(!isPanning) transform.LookAt(target);


    }
}
