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
        html(): string;
        html(val: string): void;
        fullHtml(): string;
        text(): string;
        text(val: string): void;
    }
}

declare var KindEditor: KindEditor.KindEditorStatic;