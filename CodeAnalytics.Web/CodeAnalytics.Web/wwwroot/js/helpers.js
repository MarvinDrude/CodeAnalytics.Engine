
window.scrollToElement = (el) => {
    el?.scrollIntoView({ behavior: "instant", block: "center", inline: "center" });
}

window.observeResize = (el, dotnetRef, callbackName) => {
    
    if (!el) return;   
    
    const observer = new ResizeObserver((entries) => {
        if (entries.length === 0) return;
        const last = entries[entries.length - 1];
        const { width, height } = last.contentRect;
        
        dotnetRef.invokeMethodAsync(callbackName, { Width: width, Height: height }); // maybe throttle in future
    });

    const rect = el.getBoundingClientRect();
    dotnetRef.invokeMethodAsync(callbackName, { Width: rect.width, Height: rect.height });
    
    observer.observe(el, {
        box: "border-box",
    });
}

window.enhanceDataView = () => {
    
    if (DataView.prototype.getStringEx) return;
    const decoder = new TextDecoder("utf-8");
    
    DataView.prototype.getStringEx = function (offset) {
        
        const length = this.getInt32(offset, true);
        offset += 4;
        
        if (length === 0) {
            return [offset, ""];
        }
        
        const bytes = new Uint8Array(this.buffer, this.byteOffset + offset, length);
        offset += length;
        
        return [offset, decoder.decode(bytes)];
    };
}

window.initializeCanvasRenderer = (data) => {

    window.canvasRenderers = { };
    window.enhanceDataView();
    
    const registerFunc = (entry, baseFunc) => {
        
        window.canvasRenderers[entry.rawKindValue] = (ctx, dataView, offset) => {
            try {
                return baseFunc(ctx, dataView, offset);
            } catch (error) {
                console.error(error);
            }
        };
        
    };

    for (let i = 0; i < data.length; i++) {
        
        const entry = data[i];
        const baseFunc = eval(`(${entry.jsFunction})`);

        registerFunc(entry, baseFunc);
        
    }
    
    window.handleCanvasRenderBatch = (canvas, batch) => {
      
        canvas._context2d = canvas._context2d || canvas.getContext("2d");
        
        const dataView = new DataView(batch.buffer, batch.byteOffset, batch.byteLength);
        let offset = 0;
        
        while (offset < batch.length) {

            const typeId = dataView.getUint16(offset, true); offset += 2;
            const renderer = window.canvasRenderers[typeId];
            
            offset = renderer(canvas._context2d, dataView, offset);
            
        }
        
    };
    
}