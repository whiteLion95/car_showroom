using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MultiSlider : MonoBehaviour
{
    [SerializeField] private LayoutElement blockPrefab;
    [SerializeField] private float changeSpeed;

    public List<LayoutElement> Blocks { get; private set; }

    public void Init(int blocksCount)
    {
        Clear();

        Blocks = new List<LayoutElement>();

        for (int i = 0; i < blocksCount; i++)
        {
            Blocks.Add(Instantiate(blockPrefab, transform));
            Blocks[i].flexibleWidth = 1f / blocksCount;
        }
    }

    public void SetBlockColor(LayoutElement block, Color color)
    {
        Image blockImage = block.GetComponent<Image>();

        if (blockImage)
            blockImage.color = color;
    }

    /// <summary>
    /// Меняет (сужает либо расширяет) блок мультислайдера и тем самым влияя на остальные блоки
    /// </summary>
    /// <param name="blockIndex">Индекс блока в списке Blocks</param>
    /// <param name="targetWidth">Целевоое значение flexible width для блока</param>
    public void RefreshSlider(int blockIndex, float targetWidth)
    {
        if (blockIndex >= 0 && blockIndex < Blocks.Count && targetWidth != Blocks[blockIndex].flexibleWidth)
        {
            float delta = targetWidth - Blocks[blockIndex].flexibleWidth;
            ChangeBlock(Blocks[blockIndex], delta);

            int remainingBlocksCount = Blocks.Count - 1;
            float distributedDelta = -(delta / remainingBlocksCount);

            for (int i = 0; i < Blocks.Count; i++)
            {
                if (i != blockIndex)
                {
                    ChangeBlock(Blocks[i], distributedDelta);
                }
            }
        }
    }

    private void ChangeBlock(LayoutElement block, float delta)
    {
        float duration = Mathf.Abs(delta / changeSpeed);
        float targetWidth = block.flexibleWidth + delta;
        DOTween.To(() => block.flexibleWidth, x => block.flexibleWidth = x, targetWidth, duration).onComplete +=
            () =>
            {
                if (targetWidth == 0)
                {
                    DestroyBlock(block);
                }
            };
    }

    private void DestroyBlock(LayoutElement block)
    {
        Blocks.Remove(block);
        Destroy(block.gameObject);
    }

    private void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
