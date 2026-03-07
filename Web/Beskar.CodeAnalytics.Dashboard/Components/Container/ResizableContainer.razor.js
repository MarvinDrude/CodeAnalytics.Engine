export class ResizableContainerResizer 
{
    constructor(container) 
    {
        this.container = container;
        this.isDragging = false;
        this.currentGutter = null;
        this.leftPanel = null;
        this.rightPanel = null;

        this.onMouseDown = this.onMouseDown.bind(this);
        this.onMouseMove = this.onMouseMove.bind(this);
        this.onMouseUp = this.onMouseUp.bind(this);

        this.container.addEventListener('mousedown', this.onMouseDown);
        
        window.addEventListener('mousemove', this.onMouseMove);
        window.addEventListener('mouseup', this.onMouseUp);
    }

    onMouseDown(e) 
    {
        if (e.target.classList.contains('gutter')) 
        {
            this.isDragging = true;
            this.currentGutter = e.target;
            this.leftPanel = this.currentGutter.previousElementSibling;
            this.rightPanel = this.currentGutter.nextElementSibling;
            
            this.container.style.cursor = 'col-resize';
            this.container.classList.add('is-dragging');

            e.preventDefault();
        }
    }

    onMouseMove(e) 
    {
        if (!this.isDragging || !this.leftPanel || !this.rightPanel) return;

        const rect = this.container.getBoundingClientRect();
        const totalWidth = rect.width;
        
        const mouseX = e.clientX - rect.left;
        const currentLeftRect = this.leftPanel.getBoundingClientRect();

        const leftPanelStart = currentLeftRect.left - rect.left;
        const newLeftWidthPx = mouseX - leftPanelStart;

        const newLeftPercent = (newLeftWidthPx / totalWidth) * 100;

        const leftWidthPercent = (currentLeftRect.width / totalWidth) * 100;
        const rightWidthPercent = (this.rightPanel.getBoundingClientRect().width / totalWidth) * 100;
        const combinedPercent = leftWidthPercent + rightWidthPercent;

        const newRightPercent = combinedPercent - newLeftPercent;

        if (newLeftPercent > 5 && newRightPercent > 5) 
        {
            this.leftPanel.style.flex = `0 0 ${newLeftPercent}%`;
            this.rightPanel.style.flex = `0 0 ${newRightPercent}%`;
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
};

export const initResizer = (container) => new ResizableContainerResizer(container);