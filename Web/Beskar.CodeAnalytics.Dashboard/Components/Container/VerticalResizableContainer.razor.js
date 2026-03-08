export class VerticalResizableContainerResizer 
{
    constructor(container) 
    {
        this.container = container;
        this.isDragging = false;
        this.onMouseDown = this.onMouseDown.bind(this);
        this.onMouseMove = this.onMouseMove.bind(this);
        this.onMouseUp = this.onMouseUp.bind(this);

        this.container.addEventListener('mousedown', this.onMouseDown);
        window.addEventListener('mousemove', this.onMouseMove);
        window.addEventListener('mouseup', this.onMouseUp);
    }

    onMouseDown(e) 
    {
        if (e.target.classList.contains('gutter-vertical')) 
        {
            this.isDragging = true;
            this.currentGutter = e.target;
            
            this.topPanel = this.currentGutter.previousElementSibling;
            this.bottomPanel = this.currentGutter.nextElementSibling;
            
            this.container.style.cursor = 'row-resize';
            this.container.classList.add('is-dragging');
            
            e.preventDefault();
        }
    }

    onMouseMove(e) 
    {
        if (!this.isDragging || !this.topPanel || !this.bottomPanel) return;

        const rect = this.container.getBoundingClientRect();
        const totalHeight = rect.height;
        const mouseY = e.clientY - rect.top;

        const currentTopRect = this.topPanel.getBoundingClientRect();
        const topPanelStart = currentTopRect.top - rect.top;
        const newTopHeightPx = mouseY - topPanelStart;

        const newTopPercent = (newTopHeightPx / totalHeight) * 100;
        const combinedPercent = ((currentTopRect.height + this.bottomPanel.getBoundingClientRect().height) / totalHeight) * 100;
        const newBottomPercent = combinedPercent - newTopPercent;

        if (newTopPercent > 5 && newBottomPercent > 5) {
            this.topPanel.style.flex = `0 0 ${newTopPercent}%`;
            this.bottomPanel.style.flex = `0 0 ${newBottomPercent}%`;
        }
    }

    onMouseUp() 
    {
        if (this.isDragging) 
        {
            this.isDragging = false;
            this.container.style.cursor = 'default';
            this.container.classList.remove('is-dragging');
        }
    }

    dispose() 
    {
        this.container.removeEventListener('mousedown', this.onMouseDown);
        window.removeEventListener('mousemove', this.onMouseMove);
        window.removeEventListener('mouseup', this.onMouseUp);
    }
}

export const initResizer = (container) => new VerticalResizableContainerResizer(container);