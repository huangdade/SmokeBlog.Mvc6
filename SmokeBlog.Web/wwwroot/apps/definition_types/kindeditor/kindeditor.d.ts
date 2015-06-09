declare module KindEditor {
    export interface KindEditorStatic {
        create(selector: string, options?: KindEditorOptions): KindEditor;
    }
    export interface KindEditorOptions {
        width?: any;
        height?: any;
        minWidth?: any;
        minHeight?: any;
        items?: string[];
        resizeType?: number;
    }
    export interface KindEditor {

    }
}

declare var KindEditor: KindEditor.KindEditorStatic;