/******************************************************************************
 * Spine Runtime Software License - Version 1.0
 * 
 * Copyright (c) 2013, Esoteric Software
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms in whole or in part, with
 * or without modification, are permitted provided that the following conditions
 * are met:
 * 
 * 1. A Spine Single User License or Spine Professional License must be
 *    purchased from Esoteric Software and the license must remain valid:
 *    http://esotericsoftware.com/
 * 2. Redistributions of source code must retain this license, which is the
 *    above copyright notice, this declaration of conditions and the following
 *    disclaimer.
 * 3. Redistributions in binary form must reproduce this license, which is the
 *    above copyright notice, this declaration of conditions and the following
 *    disclaimer, in the documentation and/or other materials provided with the
 *    distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using System;

namespace Spine {
	public class Bone {
		static public bool yDown;

		internal BoneData data;
		internal Bone parent;
		internal float x, y, rotation, scaleX, scaleY;
		internal float m00, m01, m10, m11;
		internal float worldX, worldY, worldRotation, worldScaleX, worldScaleY;

		public BoneData Data { get { return data; } }
		public Bone Parent { get { return parent; } }
		public float X { get { return x; } set { x = value; } }
		public float Y { get { return y; } set { y = value; } }
		public float Rotation { get { return rotation; } set { rotation = value; } }
		public float ScaleX { get { return scaleX; } set { scaleX = value; } }
		public float ScaleY { get { return scaleY; } set { scaleY = value; } }

		public float M00 { get { return m00; } }
		public float M01 { get { return m01; } }
		public float M10 { get { return m10; } }
		public float M11 { get { return m11; } }
		public float WorldX { get { return worldX; } }
		public float WorldY { get { return worldY; } }
		public float WorldRotation { get { return worldRotation; } }
		public float WorldScaleX { get { return worldScaleX; } }
		public float WorldScaleY { get { return worldScaleY; } }

		/** @param parent May be null. */
		public Bone (BoneData data, Bone parent) {
			if (data == null) throw new ArgumentNullException("data cannot be null.");
			this.data = data;
			this.parent = parent;
			SetToSetupPose();
		}

		/** Computes the world SRT using the parent bone and the local SRT. */
		public void UpdateWorldTransform (bool flipX, bool flipY) {
			Bone parent = this.parent;
			if (parent != null) {
				worldX = x * parent.m00 + y * parent.m01 + parent.worldX;
				worldY = x * parent.m10 + y * parent.m11 + parent.worldY;
				if (data.inheritScale) {
					worldScaleX = parent.worldScaleX * scaleX;
					worldScaleY = parent.worldScaleY * scaleY;
				} else {
					worldScaleX = scaleX;
					worldScaleY = scaleY;
				}
				worldRotation = data.inheritRotation ? parent.worldRotation + rotation : rotation;
			} else {
				worldX = flipX ? -x : x;
				worldY = flipY ? -y : y;
				worldScaleX = scaleX;
				worldScaleY = scaleY;
				worldRotation = rotation;
			}
			float radians = worldRotation * (float)Math.PI / 180;
			float cos = (float)Math.Cos(radians);
			float sin = (float)Math.Sin(radians);
			m00 = cos * worldScaleX;
			m10 = sin * worldScaleX;
			m01 = -sin * worldScaleY;
			m11 = cos * worldScaleY;
			if (flipX) {
				m00 = -m00;
				m01 = -m01;
			}
			if (flipY != yDown) {
				m10 = -m10;
				m11 = -m11;
			}
		}

		public void SetToSetupPose () {
			BoneData data = this.data;
			x = data.x;
			y = data.y;
			rotation = data.rotation;
			scaleX = data.scaleX;
			scaleY = data.scaleY;
		}

		override public String ToString () {
			return data.name;
		}
	}
}
