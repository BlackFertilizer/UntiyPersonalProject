﻿using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CombineMeshes : MonoBehaviour
{
    // 待合并的skinnedrender(需要所有skinnedMeshrenderer使用同一个材质,否则无法起到合并dc的目的)
    SkinnedMeshRenderer[] renders;
    // 合并后使用的材质
    public Material material;

    public SkinnedMeshRenderer [] meshes;
 
    void Start()
    {
        renders = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        // 先记录当前预制件的变换矩阵，合并之后再赋值回来
        // 待合并的skinnedrender需要记录的信息
        List<Transform> bones = new List<Transform>();
        List<BoneWeight> boneWeights = new List<BoneWeight>();
        List<CombineInstance> combineInstances = new List<CombineInstance>();
 
        int length = renders.Length;
        int boneOffset = 0;
        for (int i = 0; i < length; i++)
        {
      
            SkinnedMeshRenderer oneRender = renders[i];
            //设置统一的坐标原点
            oneRender.transform.position = Vector3.zero;
            oneRender.transform.rotation = Quaternion.identity;
            oneRender.transform.localScale = Vector3.one;

            // 记录骨骼
            bones.AddRange(oneRender.bones);
            // 记录权重
            BoneWeight[] meshBoneweight = oneRender.sharedMesh.boneWeights;
            for (int j = 0; j < meshBoneweight.Length; ++j)
            {
                BoneWeight bw = meshBoneweight[j];
                BoneWeight bWeight = bw;
                bWeight.boneIndex0 += boneOffset;
                bWeight.boneIndex1 += boneOffset;
                bWeight.boneIndex2 += boneOffset;
                bWeight.boneIndex3 += boneOffset;
                boneWeights.Add(bWeight);
            }
            // offset是为了合并之后BoneWeight.boneIndex还能正确定向骨骼
            boneOffset += oneRender.bones.Length;
            // 记录网格相关信息
            CombineInstance combineInstance = new CombineInstance();
            Mesh mesh = new Mesh();
            oneRender.BakeMesh(mesh);
            // mesh.uv = oneRender.sharedMesh.uv;
            combineInstance.mesh = mesh;
            if(meshes != null && meshes.Length > 0 &&  meshes[i]!=null)
                meshes[i].sharedMesh = mesh;
            combineInstance.transform =  transform.worldToLocalMatrix ;
            combineInstances.Add(combineInstance);
            oneRender.gameObject.SetActive(false);
        }
 
        // 将所有的骨骼变换矩阵从自身转换到当前预制件下
        List<Matrix4x4> bindposes = new List<Matrix4x4>();
        int boneLength = bones.Count;
        for (int i = 0; i < boneLength; i++)
        {
            bindposes.Add(bones[i].worldToLocalMatrix * transform.localToWorldMatrix);
        }
 
        // 在当前预制下创建新的蒙皮渲染器,设置属性
        SkinnedMeshRenderer combinedSkinnedRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances.ToArray(), true, true);
        combinedSkinnedRenderer.sharedMesh = combinedMesh;
        combinedSkinnedRenderer.bones = bones.ToArray();
        combinedSkinnedRenderer.sharedMesh.boneWeights = boneWeights.ToArray();
        combinedSkinnedRenderer.sharedMesh.bindposes = bindposes.ToArray();
        combinedSkinnedRenderer.sharedMesh.RecalculateBounds();
        combinedSkinnedRenderer.material = material;
 
    }
}