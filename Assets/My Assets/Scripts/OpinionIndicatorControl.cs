using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam2018
{
    public class OpinionIndicatorControl : MonoBehaviour
    {
        OpinionStatus _opinion;
        UnityEngine.UI.Image _image;

        Color _defaultColor = new Color(0.75f, 0.75f, 0.75f);

        // Use this for initialization
        void Start()
        {
            this._opinion = this.GetComponentInParent<OpinionStatus>();
            this._image = this.GetComponent<UnityEngine.UI.Image>();
        }

        // Update is called once per frame
        void Update()
        {
            this.GetComponent<Transform>().rotation = Quaternion.Euler(-35.802f, 54.667f, 36.887f);

            Opinion highestOpinion = this._opinion.GetHighestOpinion();

            if (highestOpinion == null)
            {
                this._image.color = new Color(this._defaultColor.r, this._defaultColor.g, this._defaultColor.b);
            }
            else
            {
                this._image.color = Color.LerpUnclamped(this._defaultColor, highestOpinion.Colour, highestOpinion.Percent / 0.5f);
            }
        }
    }
}
