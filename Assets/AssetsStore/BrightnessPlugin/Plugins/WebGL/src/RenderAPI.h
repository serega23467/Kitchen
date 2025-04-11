#pragma once

#include "Unity/IUnityGraphics.h"

#include <stddef.h>

struct IUnityInterfaces;

class RenderAPI
{
public:
	virtual ~RenderAPI() { }

	virtual void ProcessDeviceEvent(UnityGfxDeviceEventType type, IUnityInterfaces* interfaces) = 0;

	virtual bool GetUsesReverseZ() = 0;

	virtual void DrawSimpleTriangles(const float worldMatrix[16], int triangleCount, const void* verticesFloat3Byte4, bool negative) = 0;

	virtual void* BeginModifyTexture(void* textureHandle, int textureWidth, int textureHeight, int* outRowPitch) = 0;

	virtual void EndModifyTexture(void* textureHandle, int textureWidth, int textureHeight, int rowPitch, void* dataPtr) = 0;

	virtual void* BeginModifyVertexBuffer(void* bufferHandle, size_t* outBufferSize) = 0;

	virtual void EndModifyVertexBuffer(void* bufferHandle) = 0;
};

RenderAPI* CreateRenderAPI(UnityGfxRenderer apiType);

