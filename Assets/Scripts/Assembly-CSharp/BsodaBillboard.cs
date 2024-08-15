using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
[ExecuteInEditMode]
public class BsodaBillboard : MonoBehaviour
{
	// Token: 0x0600002A RID: 42 RVA: 0x00002B70 File Offset: 0x00000D70
	private void Start()
	{
		this.c = Camera.main;
		this.t = this.c.transform;
		this.x = (this.maintainXZRot ? base.transform.eulerAngles.x : 0f);
		this.z = (this.maintainXZRot ? base.transform.eulerAngles.z : 0f);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void OnBecameVisible()
	{
		this.SetRotation();
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002BEB File Offset: 0x00000DEB
	private void OnWillRenderObject()
	{
		this.SetRotation();
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002BF4 File Offset: 0x00000DF4
	private void SetRotation()
	{
		if (this.c == null)
		{
			return;
		}
		base.transform.rotation = (this.maintainXZRot ? Quaternion.Euler(this.x, this.t.eulerAngles.y, this.z) : new Quaternion(this.x, this.t.rotation.y, this.z, this.t.rotation.w));
	}

	// Token: 0x0400003D RID: 61
	public bool maintainXZRot;

	// Token: 0x0400003E RID: 62
	private Camera c;

	// Token: 0x0400003F RID: 63
	private Transform t;

	// Token: 0x04000040 RID: 64
	[NonSerialized]
	public float x;

	// Token: 0x04000041 RID: 65
	[NonSerialized]
	public float z;
}
