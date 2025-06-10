export interface PaginatedResponse<T> {
    items: T[];
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
}

export class PaginationParams {
    page: number;
    pageSize: number;

    constructor(page: number = 1, pageSize: number = 10) {
        this.page = page;
        this.pageSize = pageSize;
    }

    toHttpParams(): { [param: string]: string } {
        return {
            page: this.page.toString(),
            pageSize: this.pageSize.toString()
        };
    }
}

export interface PaginationFilter {
    specializations: number[],
    minExperienceYears?: number,
    language?: string,
    strictSpecializationFilter: boolean
    // Add other filter properties as needed
}